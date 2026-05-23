namespace SMS.Application.Interfaces.Helpers
{
    public interface IImageHelper
    {
        string ResolveImagePath(string directory, Guid imageGuid);
    }
}
