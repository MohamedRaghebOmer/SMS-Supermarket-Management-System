using SMS.Core;
using SMS.Core.DTOs.Enums;
using SMS.Core.Interfaces;
using System;
using System.Threading.Tasks;

namespace SMS.Service
{
    internal class Helper
    {
        public async Task HandelError<T>(DBResponse<T> result, string source, ILogger logger)
        {
            if (result.Code == StatusCode.UnexpectedError)
            {
                await logger.LogAsync(LogLevel.Error, source, result.Message);
            }
        }

        public async Task HandelError<T>(Exception ex, DBResponse<T> result, string source, IDatabaseLogger logger)
        {
            result.Code = StatusCode.UnexpectedError;
            result.Message = ex.Message;

            await logger.LogAsync(LogLevel.Error, $"Error while adding new country: {ex.Message}", ex, source);
        }
    }
}
