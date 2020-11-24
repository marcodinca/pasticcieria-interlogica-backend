using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace PasticceriaInterlogicaBackend.DataAccess
{
    public static class LoginDataAccess
    {
        public static string ConnectionString = Startup.StaticConfig.GetValue<string>("ConnectionStrings:DbConnection");

        public static bool Login(string email, string pwd)
        {
            using(SqlConnection Connection = new SqlConnection(ConnectionString))
            {
                string Query = @"SELECT COUNT(Id)
                                 FROM Amministratori
                                 WHERE Email = @email
                                 AND Pwd = @pwd";

                return Connection.ExecuteScalar<int>(Query, new { email = email, pwd = pwd }) > 0;
            }
        }
    }
}
