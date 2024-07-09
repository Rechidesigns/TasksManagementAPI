using TasksManagementAPI.Core.Entities.Dto;
using TasksManagementAPI.Core.Interface;

namespace TasksManagementAPI.Core.Interface
{
    public interface IResponseService
    {
        ResponseDto<T> CreateResponse<T>(T data, string message);
    }
}
