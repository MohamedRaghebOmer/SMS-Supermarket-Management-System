using SMS.Core.Enums;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SMS.Core.Logging
{
    public class Logger
    {
        public delegate Task LogEventHandler(object sender, LoggerEventArgs e);
        private event LogEventHandler _logEvent;

        public class LoggerEventArgs : EventArgs
        {
            public LogLevel Level { get; }
            public string Message { get; }
            public Exception Exception { get; }
            public string Source { get; }

            public LoggerEventArgs(LogLevel level, string message, Exception exception = null, string source = null)
            {
                if (!Enum.IsDefined(typeof(LogLevel), level))
                {
                    throw new ArgumentOutOfRangeException(nameof(level), "Invalid log level.");
                }

                if (string.IsNullOrWhiteSpace(message))
                {
                    throw new ArgumentNullException(nameof(message), "Log message cannot be empty.");
                }

                Level = level;
                Message = message;
                Exception = exception;
                Source = source;
            }
        }


        public void Subscribe(LogEventHandler handler)
        {
            if (handler == null)
            {
                throw new ArgumentNullException(nameof(handler));
            }

            _logEvent += handler;
        }

        public void Unsubscribe(LogEventHandler handler)
        {
            if (handler == null)
            {
                throw new ArgumentNullException(nameof(handler));
            }

            _logEvent -= handler;
        }

        private async Task SafeInvoke(LogEventHandler handler, LoggerEventArgs e)
        {
            try
            {
                await handler(this, e);
            }
            catch
            {
                // ignore logging failures
                // Never throw from a logger, otherwise you risk crashing the app when logging fails
            }
        }

        public async Task LogAsync(LoggerEventArgs e)
        {
            if (e == null)
            {
                throw new ArgumentNullException(nameof(e));
            }

            var handlers = _logEvent;
            if (handlers == null)
                return;

            var invocationList = handlers
            .GetInvocationList()
            .Cast<LogEventHandler>()
            .ToList();

            var tasks = invocationList.Select(h => SafeInvoke(h, e));

            await Task.WhenAll(tasks);
        }


        // Constructors
        public Logger() { }

        public Logger(LogEventHandler handler)
        {
            if (handler == null)
                throw new ArgumentNullException(nameof(handler));

            Subscribe(handler);
        }

        public Logger(params LogEventHandler[] handlers)
        {
            if (handlers == null)
                throw new ArgumentNullException(nameof(handlers));

            foreach (var handler in handlers)
            {
                if (handler != null)
                    Subscribe(handler);
            }
        }
    }
}