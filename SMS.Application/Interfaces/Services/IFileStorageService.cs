using Microsoft.AspNetCore.Http;

namespace SMS.Application.Interfaces.Services
{
    public interface IFileStorageService
    {
        Task<string> SaveFileAsync(IFormFile file, string directory);
        Stream LoadFile(string filePath);
        Task ReplaceFileAsync(Guid OldFileNameGuid, IFormFile newFile, string directory);
        Task DeleteFileAsync(string filePath);
    }
}
