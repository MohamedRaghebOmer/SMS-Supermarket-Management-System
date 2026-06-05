namespace SMS.Domain.Entities
{
    public sealed class Role
    {
        /*
                        (Roles)

           RoleId (PK, int, not null)
           RoleName (nvarchar(50), not null)
           RoleDescription (nvarchar(250), null)
           IsActive (bit, not null)
           CreatedAt (datetime2(7), not null)
         */

        private int _roleId;
        private string _roleName = null!;
        private string? _roleDescription;

        public int RoleId
        {
            get => _roleId;
            set
            {
                SMS.Shared.Guards.NumericGuard.AgainstInvalidId(value);
                _roleId = value;
            }
        }

        public string RoleName
        {
            get => _roleName;
            set
            {
                SMS.Shared.Guards.StringGuard.AgainstNullOrWhiteSpace(value, nameof(RoleName));
                SMS.Shared.Guards.StringGuard.AgainstExcessiveLength(value, 50, nameof(RoleName));
                _roleName = value.Trim();
            }
        }

        public string? RoleDescription
        {
            get => _roleDescription;
            set
            {
                SMS.Shared.Guards.StringGuard.AgainstExcessiveLength(value, 250, nameof(RoleDescription));
                _roleDescription = string.IsNullOrWhiteSpace(value) ? null : value.Trim();
            }
        }

        public bool IsActive { get; set; }

        public Role() { }

        public Role(string roleName, string? roleDescription, bool isActive)
        {
            RoleName = roleName;
            RoleDescription = roleDescription;
            IsActive = isActive;
        }

        public Role(int roleId, string roleName, string? roleDescription, bool isActive)
            : this(roleName, roleDescription, isActive)
        {
            RoleId = roleId;
        }
    }
}
