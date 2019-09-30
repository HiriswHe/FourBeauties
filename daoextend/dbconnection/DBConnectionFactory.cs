using daoextend.enums;
using daoextend.interfaces;
using daoextend.utils;
using MySql.Data.MySqlClient;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace daoextend.dbconnection
{
    public static class DBConnectionFactory
    {
        public static string CurrentDirectory { get; set; } = Environment.CurrentDirectory + "\\";
        public static IDbConnection GetDbConnection(string connectionString, DBServerType dbType = DBServerType.SqlServer)
        {
            IDbConnection dbConnection = null;
            switch (dbType )
            {
                case DBServerType.SqlServer:
                    dbConnection = new SqlConnection(connectionString);
                    break;
                case DBServerType.MySql:
                    dbConnection = new MySqlConnection(connectionString);
                    break;
                case DBServerType.Oracle:
                    dbConnection = new OracleConnection(connectionString);
                    break;
                default:
                    dbConnection = new SqlConnection(connectionString);
                    break;
            }
            return dbConnection;
        }
    }
}
