using SMS.Core.DTOs.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace SMS.Core.DTOs
{
    public class User
    {
        public int UserId { get; internal set; }
        public int PersonId { get; set; }
        public Person Person { get; internal set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public string TokenHash { get; set; }
        public int RoleId { get; set; }
        public Role Role { get; internal set; }
        public bool IsActive { get; set; }
        public DateTime LastLoginAt { get; internal set; }
        public DateTime CreatedAt { get; internal set; }
        public DateTime UpdatedAt { get; internal set; }
        public EntityMode Mode { get; internal set; } = EntityMode.AddNew;


        public User() { }

        public User(int personId, string username, string passwordHash, string tokenHash, int roleId, bool isActive)
        {
            PersonId = personId;
            Username = username;
            PasswordHash = passwordHash;
            TokenHash = tokenHash;
            RoleId = roleId;
            IsActive = isActive;
        }

        internal User(int userId, int personId, Person person, string username, string passwordHash, string tokenHash, int roleId, Role role, bool isActive, DateTime lastLoginAt, DateTime createdAt, DateTime updatedAt)
        {
            UserId = userId;
            PersonId = personId;
            Person = person;
            Username = username;
            PasswordHash = passwordHash;
            TokenHash = tokenHash;
            RoleId = roleId;
            Role = role;
            IsActive = isActive;
            LastLoginAt = lastLoginAt;
            CreatedAt = createdAt;
            UpdatedAt = updatedAt;
            Mode = EntityMode.Update;
        }
    }
}
