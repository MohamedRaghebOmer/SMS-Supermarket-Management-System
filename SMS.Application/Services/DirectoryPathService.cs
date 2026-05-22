using Microsoft.AspNetCore.Hosting;
using SMS.Application.Interfaces.Services;

namespace SMS.Application.Services
{
    public class DirectoryPathService : IDirectoryPathService
    {
        private readonly IWebHostEnvironment _env;

        public DirectoryPathService(IWebHostEnvironment env)
        {
            _env = env;
        }

        public string BaseDirectory => _env.WebRootPath;

        public string PeopleDirectory => Path.Combine(BaseDirectory, "people");

        public string ProductsDirectory => Path.Combine(BaseDirectory, "products");
    }
}