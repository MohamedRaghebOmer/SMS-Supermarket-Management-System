using SMS.Application.Common.Enums;

namespace SMS.Application.Common.Results
{
    public class OperationResult<T>
    {
        public T? Data { get; set; }
        public OperationStatus Status { get; set; } = OperationStatus.Success;
        public string Message { get; set; } = string.Empty;
        public bool IsSuccess => Status == OperationStatus.Success;


        public void ThrowIfNotSuccess()
        {
            if (IsSuccess) return;

            if (Status == OperationStatus.ValidationError)
            {
                throw new Exceptions.ValidationException(Message);
            }
            else if (Status == OperationStatus.NotFound)
            {
                throw new Exceptions.NotFoundException(Message);
            }
            else
            {
                throw new Exception(Message);
            }
        }

        public void ThrowIfValidationError()
        {
            if (Status == OperationStatus.ValidationError)
            {
                throw new Exceptions.ValidationException(Message);
            }
        }

        public void ThrowIfNotFound()
        {
            if (Status == OperationStatus.NotFound)
            {
                throw new Exceptions.NotFoundException(Message);
            }
        }

        public void ThrowIfUnexpectedError()
        {
            if (Status == OperationStatus.UnexpectedError)
            {
                throw new Exception(Message);
            }
        }

        public void ThrowNotFoundIfDataNull()
        {
            if (Data is null)
            {
                throw new Exceptions.NotFoundException(Message);
            }
        }


        public OperationResult(T data, OperationStatus status, string message)
        {
            this.Data = data;
            this.Status = status;
            this.Message = message;
        }

        public OperationResult(OperationStatus status, string message)
        {
            this.Data = default;
            this.Status = status;
            this.Message = message;
        }

        public OperationResult()
        {
            this.Data = default;
            this.Status = OperationStatus.Success;
            this.Message = string.Empty;
        }
    }
}
