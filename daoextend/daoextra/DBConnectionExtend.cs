using daoextend.daoextend;
using daoextend.dbconnection;
using daoextend.enums;
using daoextend.interfaces;
using daoextend.utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace daoextend.daoextra
{
    public static class DBConnectionExtend
    {
        public static DBServerType GetDBServerType(this ICURDProperties cURDProperties, int id = 0)
        {
            var tableAttribute = cURDProperties.GetMatchedTableAttribute();
            if (tableAttribute == null) throw new Exception("TableAttribute Not Found");
            DBServerType dBServerType = tableAttribute.DBServerType;
            return dBServerType;
        }

        public static IDbConnection GetDBConnection(this ICURDProperties cURDProperties, int id = 0)
        {
            var tableAttribute = cURDProperties.GetMatchedTableAttribute();
            if (tableAttribute == null) throw new Exception("TableAttribute Not Found");
            DBServerType dBServerType = tableAttribute.DBServerType;
            string connectionKey = tableAttribute.ConnectionKey;
            string connectionString = AppSetting.GetConfig(connectionKey);
            string connectionStringFormat = connectionString;
            var IDataBaseSharding = cURDProperties as IDataBaseSharding;
            if (IDataBaseSharding != null) connectionStringFormat = string.Format(connectionString, IDataBaseSharding.__DataBaseIndex__);
            return DBConnectionFactory.GetDbConnection(connectionStringFormat, dBServerType);
        }

    }
}
