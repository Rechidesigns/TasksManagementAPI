using AuthService.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using TasksManagementAPI.Core.Entities.Dto;
using TasksManagementAPI.Core.Entities.Model;
using TasksManagementAPI.Data;
using TasksManagementAPI.Infrastructure.Authentication.AuthLogics.AuthInterface;
using TasksManagementAPI.Infrastructure.Authentication.Entities.AuthDto;
using TasksManagementAPI.Infrastructure.Authentication.Entities.AuthModel;

namespace TasksManagementAPI.Infrastructure.Authentication.AuthLogics.AuthService
{
    public class AuthServices : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly AppDbContext _context;
        private readonly JwtConfig _jwtConfig;
        private readonly IMailSender _mailSender;

        public AuthServices(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            JwtConfig jwtConfig,
            AppDbContext context,
            IMailSender mailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtConfig = jwtConfig;
            _context = context;
            _mailSender = mailSender;
        }

        public async Task<Result<LoginResponseDto>> Login(LoginDto model)
        {
            try
            {
                var existingUser = await _userManager.FindByEmailAsync(model.Email);
                if (existingUser == null)
                {
                    return Result<LoginResponseDto>.Failure("Invalid email or password.");
                }

                var loginResult = await _signInManager.PasswordSignInAsync(existingUser, model.Password, isPersistent: true, lockoutOnFailure: false);
                if (!loginResult.Succeeded)
                {
                    return Result<LoginResponseDto>.Failure("Invalid email or password.");
                }

                var userRefreshToken = await PersistRefreshToken(existingUser, GenerateRefreshToken());
                var response = new LoginResponseDto()
                {
                    Token = await GenerateToken(existingUser),
                    UserId = existingUser.Id,
                    FullName = $"{existingUser.FirstName} {existingUser.LastName}",
                    Email = existingUser.Email,
                    RefreshToken = userRefreshToken.RefreshToken,
                    RefreshTokenExpiryTime = userRefreshToken.RefreshTokenExpiryTime,
                };
                return Result<LoginResponseDto>.Success(response);
            }
            catch (Exception ex)
            {
                return Result<LoginResponseDto>.Failure($"An error occurred during the login process: {ex.Message}");
            }
        }

        private async Task<PersistedLogin> PersistRefreshToken(ApplicationUser user, string refreshToken)
        {
            var userRefreshToken = await _context.PersistedLogins.FirstOrDefaultAsync(c => c.UserId == new Guid(user.Id));
            if (userRefreshToken == null)
            {
                userRefreshToken = new PersistedLogin()
                {
                    UserId = new Guid(user.Id),
                    RefreshToken = refreshToken,
                    RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(_jwtConfig.RefreshTokenValidityinDays)
                };
                _context.PersistedLogins.Add(userRefreshToken);
                await _context.SaveChangesAsync();
            }
            else
            {
                userRefreshToken.RefreshToken = refreshToken;
                userRefreshToken.LastUpdatedOn = DateTime.UtcNow;
                userRefreshToken.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(_jwtConfig.RefreshTokenValidityinDays);
                _context.PersistedLogins.Update(userRefreshToken);
                await _context.SaveChangesAsync();
            }

            return userRefreshToken;
        }

        private static string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        public async Task<string> GenerateToken(ApplicationUser user)
        {
            try
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iss, _jwtConfig.ValidIssuer),
                    new Claim(JwtRegisteredClaimNames.Aud, _jwtConfig.ValidAudience),
                    new Claim("token_type", "access")
                };

                var userRoles = await _userManager.GetRolesAsync(user);
                claims.AddRange(userRoles.Select(role => new Claim(ClaimTypes.Role, role)));

                var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfig.SecretKey));
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(claims),
                    Expires = DateTime.UtcNow.AddHours(24),
                    SigningCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256Signature),
                    Issuer = _jwtConfig.ValidIssuer,
                    Audience = _jwtConfig.ValidAudience
                };

                var tokenHandler = new JwtSecurityTokenHandler();
                var token = tokenHandler.CreateToken(tokenDescriptor);

                return tokenHandler.WriteToken(token);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error generating token: {ex.Message}", ex);
            }
        }

        public async Task<Result<LoginResponseDto>> RefreshToken(RefreshTokenNewRequestModel tokenModel)
        {
            try
            {
                var accessToken = tokenModel.AccessToken;
                var refreshToken = tokenModel.RefreshToken;

                var principal = GetPrincipalFromExpiredToken(accessToken);
                if (principal == null)
                {
                    return Result<LoginResponseDto>.Failure("Invalid access token or refresh token");
                }

                var emailClaim = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);
                if (emailClaim == null)
                {
                    return Result<LoginResponseDto>.Failure("Invalid access token or refresh token");
                }

                var existingUser = await _userManager.FindByEmailAsync(emailClaim.Value);
                if (existingUser == null)
                {
                    return Result<LoginResponseDto>.Failure("Invalid access token or refresh token");
                }

                var userRefreshToken = await _context.PersistedLogins.FirstOrDefaultAsync(c => c.UserId == new Guid(existingUser.Id));
                if (userRefreshToken == null)
                {
                    return Result<LoginResponseDto>.Failure("Invalid token");
                }

                if (userRefreshToken.RefreshToken != refreshToken || userRefreshToken.RefreshTokenExpiryTime <= DateTime.Now)
                {
                    return Result<LoginResponseDto>.Failure("Invalid access token or refresh token");
                }

                var newAccessToken = await GenerateToken(existingUser);
                var newRefreshToken = GenerateRefreshToken();

                userRefreshToken = await PersistRefreshToken(existingUser, newRefreshToken);

                var response = new LoginResponseDto
                {
                    Token = newAccessToken,
                    UserId = existingUser.Id,
                    FullName = $"{existingUser.FirstName} {existingUser.LastName}",
                    Email = existingUser.Email,
                    RefreshToken = newRefreshToken,
                    RefreshTokenExpiryTime = userRefreshToken.RefreshTokenExpiryTime
                };
                return Result<LoginResponseDto>.Success(response);
            }
            catch (Exception ex)
            {
                return Result<LoginResponseDto>.Failure($"An error occurred while refreshing the token: {ex.Message}");
            }
        }

        private ClaimsPrincipal? GetPrincipalFromExpiredToken(string? token)
        {
            try
            {
                var tokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = false,
                    ValidateIssuer = false,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfig.SecretKey)),
                    ValidateLifetime = false  // Set to true if you want to validate token lifetime
                };

                var tokenHandler = new JwtSecurityTokenHandler();
                var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
                if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                {
                    throw new SecurityTokenException("Invalid token");
                }

                return principal;
            }
            catch (Exception ex)
            {
                // Log the exception to investigate further
                Console.WriteLine($"Error validating token: {ex.Message}");
                throw;
            }
        }


        public async Task<string> ChangePassword(string email, ChangePassword model)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(email);
                if (user == null) return "User doesn't exist";

                if (!await _userManager.CheckPasswordAsync(user, model.OldPassword))
                    return "Old password is incorrect";

                var result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
                if (!result.Succeeded)
                {
                    var errors = string.Join(" ", result.Errors.Select(e => e.Description));
                    return errors;
                }

                var emailModel = new EmailMessageModel
                {
                    Email = email,
                    FirstName = user.FirstName,
                    LastName = user.LastName
                };

                await _mailSender.ChangePassword(emailModel);
                return "User password successfully changed";
            }
            catch (Exception ex)
            {
                return $"An error occurred: {ex.Message}";
            }
        }
        //Task<Result<LogoutRequestDto>> Logout(string AccessToken);
        public async Task<Result<string>> Logout(string accessToken)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var token = tokenHandler.ReadJwtToken(accessToken);

                var userIdClaim = token.Claims.FirstOrDefault(claim => claim.Type == JwtRegisteredClaimNames.NameId);
                if (userIdClaim == null)
                {
                    return Result<string>.Failure("Invalid token: 'nameid' claim is missing.");
                }

                var userId = userIdClaim.Value;
                var userRefreshToken = await _context.PersistedLogins.FirstOrDefaultAsync(c => c.UserId == new Guid(userId));

                if (userRefreshToken == null)
                {
                    return Result<string>.Failure("Invalid token: Refresh token not found.");
                }

                _context.PersistedLogins.Remove(userRefreshToken);
                await _context.SaveChangesAsync();

                return Result<string>.Success("Logout successful.");
            }
            catch (Exception ex)
            {
                return Result<string>.Failure($"An error occurred during logout: {ex.Message}");
            }
        }

    }
}
