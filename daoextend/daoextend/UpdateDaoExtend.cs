using daoextend.attributes;
using daoextend.consts;
using daoextend.dbconnection;
using daoextend.enums;
using daoextend.interfaces;
using daoextend.utils;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;

namespace daoextend.daoextend
{
    public static class UpdateDaoExtend
    {
        public static IDbConnection GetDBConnection(this ICURDProperties cURDProperties,int id=0)
        {
            var tableAttribute = cURDProperties.GetMatchedTableAttribute();
            if (tableAttribute == null) throw new Exception("TableAttribute Not Found");
            DBServerType dBServerType = tableAttribute.DBServerType;
            string connectionKey = tableAttribute.ConnectionKey;
            string connectionString = AppSetting.GetConfig(connectionKey);
            return DBConnectionFactory.GetDbConnection(connectionString, dBServerType);
        }

        public static bool UpdatePropertiesByKey(this IUpdateProperties updateProperties,int id=0, string tableIndex = "", List<List<object>> listsIn = null, params string[] properties)
        {
            try
            {
                if (updateProperties == null) return false;
                
                using (IDbConnection dbConnection = updateProperties.GetDBConnection(id))
                {
                    dbConnection.Open();
                    var sql = updateProperties.GetUpdateSql(id,tableIndex, listsIn, properties);
                    var result = dbConnection.Execute(sql, updateProperties) > 0;
                    if (!result) throw new Exception("没有匹配的记录");
                    return result;
                }
            }
            catch (Exception ex)
            { throw ex; }
        }

        public static string GetUpdateSql(this IUpdateProperties updateProperties, int id = 0, string tableIndex = "", List<List<object>> listsIn = null, params string[] properties)
        {
            StringBuilder builder = new StringBuilder();
            var tableName = updateProperties.GetTableName(tableIndex);
            builder.AppendLine(string.Format("Update {0} Set ", tableName));
            if (properties != null && properties.Length > 0)
                foreach (var property in properties)
                {
                    builder.Append(string.Format("{0} = @{0},", property));
                }
            else
            {
                var propertiesAll = updateProperties.GetType().GetProperties();
                if (propertiesAll != null)
                    foreach (var property in propertiesAll)
                    {
                        if (property.IgnoreProperty(updateProperties, id)) continue;
                        string columnName = property.GetPropertyAliasName(id);
                        builder.Append(string.Format("{0} = @{1},",columnName, property.Name));
                    }
            }
            builder.Length = builder.Length - 1;
            var matchedKeys = updateProperties.GetInMatchedKeyNameAndValues(id,false,listsIn?.ToArray());
            builder.AppendJoin(" ",matchedKeys);
            return builder.ToString();
        }

    }
}
