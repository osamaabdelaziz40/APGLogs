namespace APGLogs.Services.Api.Messages
{
    public class GenericSuccessResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; }

        public T data { get; set; }
    }
}
