using Microsoft.IdentityModel.Tokens;
using SMS.Application.Common.Results;
using SMS.Application.Interfaces.DataAccess;
using SMS.Application.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace SMS.Infrastructure.Repositories
{
    public class RolesRepository : IRolesRepository
    {
        private readonly IStoredProcedureExecutor _executor;

        public RolesRepository(IStoredProcedureExecutor executor)
        {
            _executor = executor;
        } 

        public async Task<OperationResult<string?>> FindRoleNameByIdAsync(int roleId)
        {
            await using var conn = _executor.CreateConnection();
            await using var cmd = _executor.CreateCommand(conn, "usp_Roles_FindRoleNameById");

            cmd.Parameters.Add("@RoleId", System.Data.SqlDbType.Int).Value = roleId;

            return await _executor.ExecuteScalarAsync<string?>(cmd, conn);
        }
    }
}