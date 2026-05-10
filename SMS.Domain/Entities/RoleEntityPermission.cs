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
                NumericGuard.AgainstNonPositiveNumber(value, nameof(PermissionsMask));
                _permissionsMask = value;
            }
        }


        public RoleEntityPermission() { }

        public RoleEntityPermission(int roleId, SystemEntity systemEntity, int permissionsMask)
        {
            RoleId = roleId;
            Entity = systemEntity;
            PermissionsMask = permissionsMask;
        }
    }
}
