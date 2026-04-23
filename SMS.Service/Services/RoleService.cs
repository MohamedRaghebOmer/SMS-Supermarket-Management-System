using SMS.Core;
using SMS.Core.DTOs;
using SMS.Repository;
using System;
using System.Data;
using System.Threading.Tasks;

namespace SMS.Service
{
    public class RoleService : IService<Role>
    {
        private readonly RoleRepository _roleRepository;
        private readonly Helper _helper;

        public async Task<DBResponse<int>> AddAsync(Role role)
        {
            if (role == null)
            {
                throw new ArgumentNullException(nameof(role), "Role can not be null.");
            }

            if (role.Mode != Core.Enums.EntityMode.AddNew)
            {
                throw new ArgumentException("Role already exists.", nameof(role));
            }

            if (string.IsNullOrWhiteSpace(role.RoleName))
            {
                throw new ArgumentException("Role name is required.", nameof(role.RoleName));
            }

            role.RoleName = role.RoleName.Trim();

            var result = new DBResponse<int>();

            try
            {
                result = await _roleRepository.AddAsync(role);
                await _helper.HandelError(result);
            }
            catch (Exception ex)
            {
                await _helper.HandelError(ex, result, "Error occurred while adding new role.");
            }

            return result;
        }

        public async Task<DBResponse<Role>> FindAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(id), "Invalid role id.");
            }

            var result = new DBResponse<Role>();

            try
            {
                result = await _roleRepository.GetAsync(id);
                await _helper.HandelError(result);
            }
            catch (Exception ex)
            {
                await _helper.HandelError(ex, result, "Error while fetching role with id: " + id);
            }

            return result;
        }

        public async Task<DBResponse<bool>> ExistsAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(id), "Invalid role id");
            }

            var result = new DBResponse<bool>();

            try
            {
                result = await _roleRepository.ExistsAsync(id);
                await _helper.HandelError(result);
            }
            catch (Exception ex)
            {
                await _helper.HandelError(ex, result, "Error check role exists");
            }

            return result;
        }

        public async Task<DBResponse<DataTable>> GetAllAsync()
        {
            var result = new DBResponse<DataTable>();

            try
            {
                result = await _roleRepository.GetAllAsync();
                await _helper.HandelError(result);
            }
            catch (Exception ex)
            {
                await _helper.HandelError(ex, result, "An error occurred while retrieving all roles.");
            }

            return result;
        }

        public async Task<DBResponse<bool>> UpdateAsync(Role role)
        {
            if (role == null)
            {
                throw new ArgumentNullException(nameof(role), "Role can not be null.");
            }

            if (role.Mode != Core.Enums.EntityMode.Update)
            {
                throw new ArgumentException(nameof(role), "Role does not exist.");
            }

            if (string.IsNullOrWhiteSpace(role.RoleName))
            {
                throw new ArgumentException("Role name is required.", nameof(role.RoleName));
            }


            role.RoleName = role.RoleName.Trim();

            var result = new DBResponse<bool>();

            try
            {
                result = await _roleRepository.UpdateAsync(role);
                await _helper.HandelError(result);
            }
            catch (Exception ex)
            {
                await _helper.HandelError(ex, result, "An error occurred while updating a role.");
            }

            return result;
        }

        public async Task<DBResponse<bool>> DeleteAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(id), "Invalid role id.");
            }

            var result = new DBResponse<bool>();

            try
            {
                result = await _roleRepository.DeleteAsync(id);
                await _helper.HandelError(result);
            }
            catch (Exception ex)
            {
                await _helper.HandelError(ex, result, "An error occurred while deleting a role.");
            }

            return result;
        }


        public RoleService()
        {
            _roleRepository = new RoleRepository();
            _helper = new Helper();
        }
    }
}
