using System;

namespace SMS.Core.DTOs
{
    public class Role
    {
        public int RoleId { get; internal set; }
        public string RoleName { get; set; }
        public string RoleDescription { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; internal set; } = DateTime.Now;
        public Enums.EntityMode Mode { get; internal set; }


        public Role() { }

        public Role(string roleName, string roleDescription, bool isActive)
        {
            RoleName = roleName;
            RoleDescription = roleDescription;
            IsActive = isActive;
        }

        internal Role(int roleId, string roleName, string roleDescription, bool isActive, DateTime createdAt, Enums.EntityMode mode)
        {
            RoleId = roleId;
            RoleName = roleName;
            RoleDescription = roleDescription;
            IsActive = isActive;
            CreatedAt = createdAt;
            Mode = mode;
        }
    }
}
