using TasksManagementAPI.Core.Entities.Dto;
using TasksManagementAPI.Core.Interface;

namespace TasksManagementAPI.Core.Services
{
    public class ResponseService : IResponseService
    {
        public ResponseDto<T> CreateResponse<T>(T data, string message)
        {
            return new ResponseDto<T>
            {
                Data = data,
                Message = message
            };
        }
    }

}
