namespace SMS.Domain.Entities
{
    public class Country
    {
        private string _countryName = null!;

        public int CountryId { get; private set; }

        public string CountryName
        {
            get => _countryName;
            set
            {
                ArgumentException.ThrowIfNullOrWhiteSpace("Country name cannot be null or empty.");

                _countryName = value.Trim();
            }
        }

        public Country(int countryId, string countryName)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(countryId, nameof(countryId));

            CountryId = countryId;
            CountryName = countryName;
        }

        public Country(string countryName)
        {
            CountryName = countryName;
        }

        private Country() { }
    }
}
