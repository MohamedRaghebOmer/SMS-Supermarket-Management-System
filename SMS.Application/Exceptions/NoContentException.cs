namespace SMS.Application.Exceptions
{
    public class NoContentException : Exception
    {
        public NoContentException() : base()
        {
        }

        public NoContentException(string? message) : base(message)
        {
        }

        public NoContentException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
