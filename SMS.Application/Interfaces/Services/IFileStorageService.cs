using Microsoft.AspNetCore.Http;
using SMS.Contracts.Responses;

namespace SMS.Application.Interfaces.Services
{
    public interface IFileStorageService
    {
        Task SaveFileAsync(IFormFile file, string directory, Guid fileName);
        Task<Guid> SaveFileAsync(IFormFile file, string directory);
        Task<FileResponse> LoadFileAsync(string filePath);
        Task<Guid> ReplaceFileAsync(Guid oldFileNameGuid, IFormFile newFile,
            string directory);
        Task DeleteFileAsync(string filePath);
    }
}
