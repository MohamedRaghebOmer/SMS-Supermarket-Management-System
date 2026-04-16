using SMS.Core;
using SMS.Core.Enums;
using SMS.Core.Logging;
using SMS.Repository;
using System;
using System.Threading.Tasks;

namespace SMS.Service
{
    internal class Helper
    {
        private readonly Logger _logger;

        public static Logger.LoggerEventArgs GetLoggingArgs(string message, Exception ex = null, string source = null)
        {
            return new Logger.LoggerEventArgs(LogLevel.Error, message, ex, source ?? nameof(source));
        }

        public async Task HandelError<T>(DBResponse<T> result)
        {
            if (result.Code == StatusCode.UnexpectedError)
            {
                await _logger.LogAsync(GetLoggingArgs(result.Message, null));
            }
        }

        public async Task HandelError<T>(Exception ex, DBResponse<T> result, string loggingMessage)
        {
            result.Code = StatusCode.UnexpectedError;
            result.Message = ex.Message;

            await _logger.LogAsync(GetLoggingArgs("Error while adding new country.", ex));
        }

        public Helper()
        {
            this._logger = new Logger();
            _logger.Subscribe(async (sender, e) => await LogsRepository.LogAsync(e));
            _logger.Subscribe(async (sender, e) => await LogToWinEvents.LogAsync(e));
        }

    }
}
