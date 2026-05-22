namespace SMS.Application.Interfaces.Services
{
    public interface IDirectoryPathService
    {
        public string BaseDirectory { get; }
        public string PeopleDirectory { get; }
        public string ProductsDirectory { get; }
    }
}
