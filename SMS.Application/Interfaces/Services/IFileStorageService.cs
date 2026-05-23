using Microsoft.AspNetCore.Http;

namespace SMS.Application.Interfaces.Services
{
    public interface IFileStorageService
    {
        Task<Guid> SaveFileAsync(IFormFile file, string directory);
        Stream LoadFile(string filePath);
        Task<Guid> ReplaceFileAsync(Guid oldFileNameGuid, IFormFile newFile,
            string directory);
        Task DeleteFileAsync(string filePath);
    }
}
