using SMS.Application.Interfaces.Helpers;

namespace SMS.Application.Helpers
{
    public class StringHelper : IStringHelper
    {
        public string ToTitleCase(string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;
            var words = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < words.Length; i++)
            {
                var word = words[i];
                if (word.Length > 1)
                {
                    words[i] = char.ToUpper(word[0]) + word.Substring(1).ToLower();
                }
                else
                {
                    words[i] = word.ToUpper();
                }
            }
            return string.Join(' ', words);
        }

        public string Hash(string str)
        {
            return BCrypt.Net.BCrypt.HashPassword(str);
        }

        public bool Verify(string str, string hash)
        {
            return BCrypt.Net.BCrypt.Verify(str, hash);
        }
    }
}
