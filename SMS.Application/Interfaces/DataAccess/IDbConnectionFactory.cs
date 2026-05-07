using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Data.SqlClient;

namespace SMS.Application.Interfaces.DataAccess
{
    public interface IDbConnectionFactory
    {
        SqlConnection CreateConnection();
    }
}
