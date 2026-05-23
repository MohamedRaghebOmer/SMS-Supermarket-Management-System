using Microsoft.AspNetCore.Http;
using SMS.Application.Interfaces.Services;

namespace SMS.Application.Services
{
    public class FileStorageService : IFileStorageService
    {
        public async Task<Guid> SaveFileAsync(IFormFile file, string directory)
        {
            ArgumentNullException.ThrowIfNull(file);

            if (string.IsNullOrWhiteSpace(directory))
                throw new ArgumentException("Invalid directory path", nameof(directory));

            Directory.CreateDirectory(directory);

            var extension = Path.GetExtension(file.FileName);
            var fileNameGuid = Guid.NewGuid();
            var fileName = $"{fileNameGuid}{extension}";
            var filePath = Path.Combine(directory, fileName);

            await using var stream = new FileStream(
                filePath,
                FileMode.Create,
                FileAccess.Write,
                FileShare.None
            );

            await file.CopyToAsync(stream);

            return fileNameGuid;
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

        public async Task<Guid> ReplaceFileAsync(Guid oldFileNameGuid, IFormFile newFile,
            string directory)
        {
            if (oldFileNameGuid == Guid.Empty)
                throw new ArgumentException("Invalid file name GUID", nameof(oldFileNameGuid));

            ArgumentNullException.ThrowIfNull(newFile);

            if (string.IsNullOrWhiteSpace(directory))
                throw new ArgumentException("Invalid directory path", nameof(directory));

            Directory.CreateDirectory(directory);

            // Find the old file whatever the extension is
            var oldFilePath = Directory
                .GetFiles(directory, $"{oldFileNameGuid}.*")
                .FirstOrDefault();

            if (oldFilePath is null)
                throw new FileNotFoundException(
                    "File not found",
                    $"{oldFileNameGuid}.*"
                );

            // Delete old file first
            File.Delete(oldFilePath);

            // Generate new file name
            var newFileNameGuid = Guid.NewGuid();
            var extension = Path.GetExtension(newFile.FileName);

            var newFilePath = Path.Combine(
                directory,
                $"{newFileNameGuid}{extension}"
            );

            // Save new file
            await using var stream = new FileStream(
                newFilePath,
                FileMode.CreateNew,
                FileAccess.Write,
                FileShare.None
            );

            await newFile.CopyToAsync(stream);

            return newFileNameGuid;
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