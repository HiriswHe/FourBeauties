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
                using (IDbConnection dbConnection = cURDProperties.GetDBConnection())
                {
                    dbConnection.Open();
                    var transation = dbConnection.BeginTransaction();
                    if (parameterrs != null && parameterrs.Length > 0)
                    {
                        result = dbConnection.Execute(sql, parameterrs, transation) > 0;
                        transation.Commit();
                    }
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
