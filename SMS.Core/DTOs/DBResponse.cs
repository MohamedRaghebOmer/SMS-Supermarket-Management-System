


namespace SMS.Core
{
    public enum StatusCode
    {
        Success = 1,
        ValidationError = -1,
        NotFound = -2,
        UnexpectedError = -99
    }

    public class DBResponse <T>
    {
        public T Data { get; set; }
        public StatusCode Code { get; set; }
        public string Message { get; set; }

        public DBResponse() { }

        public DBResponse(T data, StatusCode code, string message)
        {
            this.Data = data;
            this.Code = code;
            this.Message = message;
        }
    }
}
