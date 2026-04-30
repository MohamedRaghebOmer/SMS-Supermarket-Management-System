using SMS.Core.DTOs.Enums;
using System.Threading.Tasks;

namespace SMS.Core.Interfaces
{
    public interface ILogger
    {
        Task LogAsync(LogLevel level, string source , string message);
    }
}
