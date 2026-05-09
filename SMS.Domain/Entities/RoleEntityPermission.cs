using SMS.Shared.Enums;
using SMS.Shared.Guards;
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Security.Principal;
using System.Text;

namespace SMS.Domain.Entities
{
    public class RoleEntityPermission
    {
        private int _roleId;
        private int _permissionMask;

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

        public int PermissionMask
        {
            get => _permissionMask;
            set
            {
                NumericGuard.AgainstNonPositiveNumber(value, nameof(PermissionMask));
                _permissionMask = value;
            }
        }


        public RoleEntityPermission() { }

        public RoleEntityPermission(int roleId, SystemEntity systemEntity, int permissionMask)
        {
            RoleId = roleId;
            Entity = systemEntity;
            PermissionMask = permissionMask;
        }
    }
}
