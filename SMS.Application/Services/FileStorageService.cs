using Microsoft.AspNetCore.Http;
using SMS.Application.Interfaces.Services;

namespace SMS.Application.Services
{
    public class FileStorageService : IFileStorageService
    {
        public async Task<string> SaveFileAsync(IFormFile file, string directory)
        {
            ArgumentNullException.ThrowIfNull(file);

            if (string.IsNullOrWhiteSpace(directory))
                throw new ArgumentException("Invalid directory path", nameof(directory));

            Directory.CreateDirectory(directory);

            var extension = Path.GetExtension(file.FileName);
            var fileName = $"{Guid.NewGuid()}{extension}";
            var filePath = Path.Combine(directory, fileName);

            await using var stream = new FileStream(
                filePath,
                FileMode.Create,
                FileAccess.Write,
                FileShare.None
            );

            await file.CopyToAsync(stream);

            return filePath;
        }

        public Stream LoadFile(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath) || !File.Exists(filePath))
                throw new ArgumentException("Invalid file path", nameof(filePath));

            return new FileStream(
                filePath,
                FileMode.Open,
                FileAccess.Read,
                FileShare.Read
            );
        }

        public async Task ReplaceFileAsync(Guid OldFileNameGuid, IFormFile newFile, string directory)
        {
            if (OldFileNameGuid == Guid.Empty)
                throw new ArgumentException("Invalid file name GUID", nameof(OldFileNameGuid));

            ArgumentNullException.ThrowIfNull(newFile);

            if (string.IsNullOrWhiteSpace(directory))
                throw new ArgumentException("Invalid directory path", nameof(directory));

            Directory.CreateDirectory(directory);

            var extension = Path.GetExtension(newFile.FileName);
            var filePath = Path.Combine(directory, $"{OldFileNameGuid}{extension}");

            if (!File.Exists(filePath))
                throw new FileNotFoundException("File not found", filePath);

            await using var stream = new FileStream(
                filePath,
                FileMode.Create,
                FileAccess.Write,
                FileShare.None
            );

            await newFile.CopyToAsync(stream);
        }

        public Task DeleteFileAsync(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentException("Invalid file path", nameof(filePath));

            if (!File.Exists(filePath))
                return Task.CompletedTask;

            File.Delete(filePath);

            return Task.CompletedTask;
        }
    }
}