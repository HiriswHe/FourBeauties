using daoextend.attributes;
using daoextend.interfaces;
using daoextend.utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace daoextend.daoextend
{
    public static class TableExtend
    {

        public static string GetTableName(this ICURDProperties updateProperties, string tableIndex ="", int id = 0)
        {
            var tableAttribute = updateProperties.GetCustomerAttributes<MatchedTableAttribute>().FirstOrDefault(w => w.ID == id);
            var tableName = tableAttribute?.Name;
            if (string.IsNullOrEmpty(tableName))
            {
                tableName = updateProperties.GetType().Name;
            }
            //tableName = tableName.Trim();//Support Use Sencond Type TableIndex For Table FullName
            if (tableName.StartsWith("{") && tableName.EndsWith("}"))
                tableName = AppSetting.GetConfig(tableName.TrimStart('{').TrimEnd('}'));
            if (tableName.Contains("{") && tableName.Contains("}"))
            {
                string tableNameFormat = string.Format(tableName, tableIndex);
                return tableNameFormat;
            }
            return tableName;
        }


        public static MatchedTableAttribute GetMatchedTableAttribute(this ICURDProperties updateProperties, int id = 0)
        {
            var tableAttribute = updateProperties.GetCustomerAttributes<MatchedTableAttribute>().FirstOrDefault(w => w.ID == id);
            return tableAttribute;
        }

        public static string GetConnectionKey(this ICURDProperties updateProperties, int id = 0)
        {
            var tableAttribute = updateProperties.GetCustomerAttributes<MatchedTableAttribute>().FirstOrDefault(w => w.ID == id);
            var connectionKey = tableAttribute?.ConnectionKey;
            if (string.IsNullOrEmpty(connectionKey)) return connectionKey;
            connectionKey = connectionKey.Trim();
            if (connectionKey.StartsWith('{') && connectionKey.EndsWith('}'))
                connectionKey = AppSetting.GetConfig(connectionKey.TrimStart('{').TrimEnd('}'));
            return connectionKey;
        }

    }
}
