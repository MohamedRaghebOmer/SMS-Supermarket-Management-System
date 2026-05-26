using SMS.Application.Interfaces.Services;

namespace SMS.Application.Services
{
    public class DirectoryPathService : IDirectoryPathService
    {
        private readonly IFileStoragePathProvider _fileStoragePathProvider;

        public DirectoryPathService(IFileStoragePathProvider fileStoragePathProvider)
        {
            _fileStoragePathProvider = fileStoragePathProvider;

            Directory.CreateDirectory(PeopleDirectory);
            Directory.CreateDirectory(ProductsDirectory);
        }

        public string BaseDirectory => _fileStoragePathProvider.BaseDirectory;

        public string PeopleDirectory => Path.Combine(BaseDirectory, "people");

        public string ProductsDirectory => Path.Combine(BaseDirectory, "products");
    }
}