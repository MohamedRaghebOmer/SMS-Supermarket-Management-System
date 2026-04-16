using SMS.Core.Enums;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("SMS.Repository")]

namespace SMS.Core.DTOs
{
    public class Country
    {
        private int countryId;

        public int CountryId
        {
            get => countryId;

            internal set
            {
                countryId = value;

                if (countryId > 0)
                {
                    this.Mode = EntityMode.Update;

                }
                else
                {
                    this.Mode = EntityMode.AddNew;
                }
            }
        }

        public string CountryName { get; set; }
        public EntityMode Mode { get; internal set; } = EntityMode.AddNew;


        public Country() { }

        public Country(string countryName)
        {
            this.CountryName = countryName;
        }

        internal Country(int countryId, string countryName)
        {
            this.CountryId = countryId;
            this.CountryName = countryName;
            this.Mode = EntityMode.Update;
        }
    }
}
