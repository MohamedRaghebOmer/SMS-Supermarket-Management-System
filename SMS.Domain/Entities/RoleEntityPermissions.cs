using SMS.Shared.Enums;
using SMS.Shared.Guards;

namespace SMS.Domain.Entities
{
    public sealed class RoleEntityPermissions
    {
        private int _roleId;
        private int _permissionsMask;

        public int RoleId
        {
            get => _roleId;

            private set
            {
                NumericGuard.AgainstInvalidId(value);
                _roleId = value;
            }
        }

        public SystemEntity Entity { get; set; }

        public int PermissionsMask
        {
            get => _permissionsMask;
            set
            {
                NumericGuard.AgainstNegativeNumber(value, nameof(PermissionsMask));
                _permissionsMask = value;
            }
        }


        public RoleEntityPermissions() { }

        public RoleEntityPermissions(int roleId, SystemEntity systemEntity, int permissionsMask)
        {
            RoleId = roleId;
            Entity = systemEntity;
            PermissionsMask = permissionsMask;
        }
    }
}
