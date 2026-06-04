using SMS.Shared.Enums;
using SMS.Shared.Guards;
using System.ComponentModel.DataAnnotations;

namespace SMS.Domain.Entities
{
    public sealed class Person
    {
        private int _personId;
        private string _nationalNo = null!;
        private string _firstName = null!;
        private string _secondName = null!;
        private string? _thirdName;
        private string _lastName = null!;
        private DateTime _dateOfBirth;
        private string _address = null!;
        private string _phone = null!;
        private string? _email;
        private int _nationalityCountryId;
        private DateTime _createdAt;

        public int PersonId
        {
            get => _personId;
            set
            {
                NumericGuard.AgainstInvalidId(value);
                _personId = value;
            }
        }

        public string NationalNo
        {
            get => _nationalNo;
            set
            {
                StringGuard.AgainstNullOrWhiteSpace(value, nameof(NationalNo));
                StringGuard.EnsureLengthInRange(value, 5, 20, nameof(NationalNo));
                _nationalNo = value.Trim();
            }
        }

        public string FirstName
        {
            get => _firstName;
            set
            {
                StringGuard.AgainstNullOrWhiteSpace(value, nameof(FirstName));
                StringGuard.EnsureLengthInRange(value, 1, 20, nameof(FirstName));
                _firstName = value.Trim();
            }
        }

        public string SecondName
        {
            get => _secondName;
            set
            {
                StringGuard.AgainstNullOrWhiteSpace(value, nameof(SecondName));
                StringGuard.EnsureLengthInRange(value, 1, 20, nameof(SecondName));
                _secondName = value.Trim();
            }
        }

        public string? ThirdName
        {
            get => _thirdName;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    _thirdName = null;
                }
                else
                {
                    StringGuard.EnsureLengthInRange(value, 1, 20, nameof(ThirdName));
                    _thirdName = value.Trim();
                }
            }
        }

        public string LastName
        {
            get => _lastName;
            set
            {
                StringGuard.AgainstNullOrWhiteSpace(value, nameof(LastName));
                StringGuard.EnsureLengthInRange(value, 1, 20, nameof(LastName));
                _lastName = value.Trim();
            }
        }

        public DateTime DateOfBirth
        {
            get => _dateOfBirth;
            set
            {
                DateGuard.AgainstFutureDate(value, nameof(DateOfBirth));
                DateGuard.EnsureIsAdult(value, nameof(DateOfBirth));
                _dateOfBirth = value.Date;
            }
        }

        public Gender Gender { get; set; }

        public string Address
        {
            get => _address;
            set
            {
                StringGuard.AgainstNullOrWhiteSpace(value, nameof(Address));
                StringGuard.EnsureLengthInRange(value, 5, 200, nameof(Address));
                _address = value.Trim();
            }
        }

        public string Phone
        {
            get => _phone;
            set
            {
                StringGuard.AgainstNullOrWhiteSpace(value, nameof(Phone));
                StringGuard.EnsureLengthInRange(value, 3, 20, nameof(Phone));
                _phone = value.Trim();
            }
        }

        public string? Email
        {
            get => _email;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    _email = null;
                }
                else
                {
                    StringGuard.EnsureLengthInRange(value, 5, 50, nameof(Email));
                    if (!new EmailAddressAttribute().IsValid(value))
                    {
                        throw new ArgumentException("Invalid email format.", nameof(Email));
                    }
                    _email = value.Trim();
                }
            }
        }

        public int NationalityCountryId
        {
            get => _nationalityCountryId;
            set
            {
                NumericGuard.AgainstInvalidId(value);
                _nationalityCountryId = value;
            }
        }

        public Guid? ImageGuid { get; set; }

        public DateTime CreatedAt
        {
            get => _createdAt;
            set
            {
                DateGuard.AgainstFutureDate(value, nameof(CreatedAt));
                _createdAt = value;
            }
        }

        public Person() { }

        public Person(string nationalNo, string firstName, string secondName, string? thirdName,
            string lastName, DateTime dateOfBirth, Gender gender, string address, string phone,
            string? email, int nationalityCountryId, Guid? imageGuid, DateTime createdAt)
        {
            NationalNo = nationalNo;
            FirstName = firstName;
            SecondName = secondName;
            ThirdName = thirdName;
            LastName = lastName;
            DateOfBirth = dateOfBirth;
            Gender = gender;
            Address = address;
            Phone = phone;
            Email = email;
            NationalityCountryId = nationalityCountryId;
            ImageGuid = imageGuid;
            CreatedAt = createdAt;
        }

        public Person(int personId, string nationalNo, string firstName, string secondName,
            string? thirdName, string lastName, DateTime dateOfBirth, Gender gender, string address,
            string phone, string? email, int nationalityCountryId, Guid? imageGuid, DateTime createdAt)
            : this(nationalNo, firstName, secondName, thirdName, lastName, dateOfBirth, gender,
                  address, phone, email, nationalityCountryId, imageGuid, createdAt)
        {
            PersonId = personId;
        }
    }
}
