namespace TasksManagementAPI.Core.Entities.Dto
{
    public class ResponseDto<T>
    {
        public T Data { get; set; }
        public string Message { get; set; }
    }

}
