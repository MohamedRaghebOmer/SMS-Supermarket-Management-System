using SMS.Application.Interfaces.Services;

namespace SMS.API.Helpers
{
    public class WebRootFileStoragePathProvider : IFileStoragePathProvider
    {
        public string BaseDirectory { get; }

        public WebRootFileStoragePathProvider(IHostEnvironment env)
        {
            BaseDirectory = Path.Combine(env.ContentRootPath, "Storage");
            Directory.CreateDirectory(BaseDirectory);
        }
    }
}
