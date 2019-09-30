using daoextend.dbconnection;
using daoextend.enums;
using daoextend.interfaces;
using daoextend.utils;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace daoextend.daoextend
{
    public static class CommonDaoExtend
    {
        public static bool ExecuteNonQuery(this ICURDProperties cURDProperties,string sql,params object[] parameterrs)
        {
            bool result = false;
            try
            {
                if (cURDProperties == null) return false;
                string connectionKey = cURDProperties.GetConnectionKey();
                string connectionString = CommonHelpr.GetConfig(connectionKey);
                using (IDbConnection dbConnection = cURDProperties.GetDBConnection())
                {
                    dbConnection.Open();
                    if (parameterrs != null && parameterrs.Length > 0)
                        result = dbConnection.Execute(sql, parameterrs) > 0;
                    else
                        result = dbConnection.Execute(sql) > 0;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }
    }
}
