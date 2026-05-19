using SMS.Shared.Guards;

namespace SMS.Domain.Entities
{
    public class User
    {
        private int _userId;
        private int _personId;
        private string _username = null!;
        private string _passwordHash = null!;
        private int _roleId;
        private DateTime? _lastLoginAt;
        private DateTime _createdAt;
        private DateTime? _updatedAt;


        public int UserId
        {
            get => _userId;
            set
            {
                NumericGuard.AgainstInvalidId(value);
                _userId = value;
            }
        }

        public int PersonId
        {
            get => _personId;

            set
            {
                NumericGuard.AgainstInvalidId(value);
                _personId = value;
            }
        }

        public string Username
        {
            get => _username;
            set
            {
                StringGuard.AgainstNullOrEmpty(value, nameof(Username));
                _username = value;
            }
        }

        public string PasswordHash
        {
            get => _passwordHash;

            set
            {
                StringGuard.AgainstNullOrEmpty(value, nameof(PasswordHash));
                _passwordHash = value;
            }
        }

        public int RoleId
        {
            get => _roleId;

            set
            {
                NumericGuard.AgainstInvalidId(value);
                _roleId = value;
            }
        }

        public bool IsActive { get; set; }

        public DateTime? LastLoginAt
        {
            get => _lastLoginAt;

            set
            {
                if (value.HasValue && value.Value > DateTime.UtcNow)
                {
                    throw new ArgumentException("LastLoginAt cannot be in the future.");
                }
                _lastLoginAt = value;
            }
        }

        public DateTime CreatedAt
        {
            get => _createdAt;

            set
            {
                if (value > DateTime.UtcNow)
                {
                    throw new ArgumentException("CreatedAt cannot be in the future.");
                }
                _createdAt = value;
            }
        }

        public DateTime? LastUpdatedAt
        {
            get => _updatedAt;

            set
            {
                if (value > DateTime.UtcNow)
                {
                    throw new ArgumentException("LastUpdatedAt cannot be in the future.");
                }
                _updatedAt = value;
            }
        }


        public User() { }

        public User(int personId, string username, string passwordHash, int roleId,
            bool isActive, DateTime? lastLoginAt, DateTime createdAt, DateTime? lastUpdatedAt)
        {
            PersonId = personId;
            Username = username;
            PasswordHash = passwordHash;
            RoleId = roleId;
            IsActive = isActive;
            LastLoginAt = lastLoginAt;
            CreatedAt = createdAt;
            LastUpdatedAt = lastUpdatedAt;
        }

        public User(int userId, int personId, string username, string passwordHash, int roleId,
            bool isActive, DateTime? lastLoginAt,
            DateTime createdAt, DateTime? lastUpdatedAt) : this(personId, username,
                passwordHash, roleId, isActive, lastLoginAt, createdAt, lastUpdatedAt)
        {
            UserId = userId;
        }
    }
}
