using SMS.Application.Interfaces.Helpers;

namespace SMS.Application.Helpers
{
    public class ImageHelper : IImageHelper
    {
        public string ResolveImagePath(string directory, Guid imageGuid)
        {
            if (string.IsNullOrWhiteSpace(directory))
            {
                throw new ArgumentException("Directory cannot be null or empty.", nameof(directory));
            }

            if (imageGuid == Guid.Empty)
            {
                throw new ArgumentException("Image GUID cannot be empty.", nameof(imageGuid));
            }

            var filePath = Directory.GetFiles(directory, $"{imageGuid}.*").FirstOrDefault();
            if (filePath is null)
            {
                throw new FileNotFoundException("Image file not found.", $"{imageGuid}.*");
            }

            return filePath;
        }
    }
}
