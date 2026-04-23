using SMS.Core.Enums;
using System;

namespace SMS.Core.DTOs
{
    public class Person
    {
        public int PersonId { get; internal set; }
        public string NationalNo { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string ThirdName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public Gender Gender { get; set; } = Gender.Male;
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public int CountryId { get; set; }
        public Country Country { get; internal set; }
        public Guid ImageGuid { get; set; }
        public DateTime CreatedAt { get; internal set; }
        public EntityMode Mode { get; internal set; } = EntityMode.AddNew;


        public Person() { }

        public Person(string nationalNo, string firstName, string secondName, string thirdName, string lastName,
            DateTime dateOfBirth, Gender gender, string address, string phone, int countryId, Country country,
            Guid imageGuid, string email)
        {
            this.NationalNo = nationalNo;
            this.FirstName = firstName;
            this.SecondName = secondName;
            this.ThirdName = thirdName;
            this.LastName = lastName;
            this.DateOfBirth = dateOfBirth;
            this.Gender = gender;
            this.Address = address;
            this.Phone = phone;
            this.CountryId = countryId;
            this.Country = country;
            this.ImageGuid = imageGuid;
            this.Email = email;
        }

        internal Person(int personId, string nationalNo, string firstName, string secondName, string thirdName,
            string lastName, DateTime dateOfBirth, Gender gender, string address, string phone, int countryId,
            Country country, Guid imageGuid, DateTime createdAt, string email) : this(nationalNo, firstName, secondName, thirdName, lastName, dateOfBirth,
                gender, address, phone, countryId, country, imageGuid, email)
        {
            this.PersonId = personId;
            this.CreatedAt = createdAt;
            this.Mode = EntityMode.Update;
        }
    }
}