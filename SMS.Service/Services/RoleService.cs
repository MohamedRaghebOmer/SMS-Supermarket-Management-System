using SMS.Core;
using SMS.Core.DTOs;
using SMS.Core.DTOs.Enums;
using SMS.Core.Interfaces;
using SMS.Core.Logging;
using SMS.Repository;
using System;
using System.Data;
using System.Threading.Tasks;

namespace SMS.Service
{
    public class RoleService : IService<Role>
    {
        private readonly IRepository<Role> _repo;
        private readonly Helper _helper;

        public async Task<DBResponse<int>> AddAsync(Role role)
        {
            if (role == null)
            {
                throw new ArgumentNullException(nameof(role), "Role can not be null.");
            }

            if (role.Mode != EntityMode.AddNew)
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
                result = await _repo.AddAsync(role);
                await _helper.HandelError(result, nameof(RoleService), new EventViewerLogger());
            }
            catch (Exception ex)
            {
                await _helper.HandelError(ex, result, nameof(RoleService), new LogRepository());
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
                result = await _repo.GetAsync(id);
                await _helper.HandelError(result, nameof(RoleService), new EventViewerLogger());
            }
            catch (Exception ex)
            {
                await _helper.HandelError(ex, result, nameof(RoleService), new LogRepository());
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
                result = await _repo.ExistsAsync(id);
                await _helper.HandelError(result, nameof(RoleService), new EventViewerLogger());
            }
            catch (Exception ex)
            {
                await _helper.HandelError(ex, result, nameof(RoleService), new LogRepository());
            }

            return result;
        }

        public async Task<DBResponse<DataTable>> GetAllAsync()
        {
            var result = new DBResponse<DataTable>();

            try
            {
                result = await _repo.GetAllAsync();
                await _helper.HandelError(result, nameof(RoleService), new EventViewerLogger());
            }
            catch (Exception ex)
            {
                await _helper.HandelError(ex, result, nameof(RoleService), new LogRepository());
            }

            return result;
        }

        public async Task<DBResponse<bool>> UpdateAsync(Role role)
        {
            if (role == null)
            {
                throw new ArgumentNullException(nameof(role), "Role can not be null.");
            }

            if (role.Mode != EntityMode.Update)
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
                result = await _repo.UpdateAsync(role);
                await _helper.HandelError(result, nameof(RoleService), new EventViewerLogger());
            }
            catch (Exception ex)
            {
                await _helper.HandelError(ex, result, nameof(RoleService), new LogRepository());
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
                result = await _repo.DeleteAsync(id);
                await _helper.HandelError(result, nameof(RoleService), new EventViewerLogger());
            }
            catch (Exception ex)
            {
                await _helper.HandelError(ex, result, nameof(RoleService), new LogRepository());
            }

            return result;
        }


        public RoleService(IRepository<Role> roleRepository)
        {
            _repo = roleRepository;
            _helper = new Helper();
        }
    }
}
