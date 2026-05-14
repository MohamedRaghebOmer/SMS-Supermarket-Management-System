namespace SMS.Application.Interfaces.Helpers
{
    public interface IStringHelper
    {
        string ToTitleCase(string input);
        string Hash(string str);
        bool Verify(string str, string hash);
    }
}
