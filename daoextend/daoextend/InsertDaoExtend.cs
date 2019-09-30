using daoextend.interfaces;
using daoextend.utils;
using Dapper;
using System;
using System.Data;
using System.Text;

namespace daoextend.daoextend
{
    public static class InsertDaoExtend
    {
        public static bool InsertProperties(this IInsertProperties insertProperties, int id = 0, params string[] properties)
        {
            try
            {
                if (insertProperties == null) return false;
                string connectionKey = insertProperties.GetConnectionKey();
                string connectionString = CommonHelpr.GetConfig(connectionKey);
                using (IDbConnection dbConnection = insertProperties.GetDBConnection(id))
                {
                    dbConnection.Open();
                    var sql = insertProperties.GetInsertSql(id, properties);
                    return dbConnection.Execute(sql, insertProperties) > 0;
                }
            }
            catch (Exception ex)
            { throw ex; }
        }

        public static string GetInsertSql(this IInsertProperties insertProperties, int id = 0,params string[] properties)
        {
            string result = string.Empty;
            StringBuilder builder = new StringBuilder();
            var tableName = insertProperties.GetTableName();
            builder.AppendLine(string.Format("Insert into {0}( ", tableName));
            if (string.IsNullOrEmpty(insertProperties.UUID)) insertProperties.UUID = Guid.NewGuid().ToString("N");
            StringBuilder columnNames = new StringBuilder();
            StringBuilder columnValues = new StringBuilder();
            if (properties != null && properties.Length > 0)
                foreach (var property in properties)
                {
                    columnNames.Append(property + ",");
                    columnValues.Append("@" + property + ",");
                }
            else
            {
                var propertiesAll = insertProperties.GetType().GetProperties();
                if (propertiesAll != null)
                    foreach (var property in propertiesAll)
                    {
                        if (property.IgnoreProperty(insertProperties, id)) continue;
                        string columnName = property.GetPropertyAliasName(id);
                        columnNames.Append(columnName + ",");
                        columnValues.Append("@"+property.Name + ",");
                    }
            }
            if(columnNames.Length>0)
            columnNames.Length = columnNames.Length - 1;
            if(columnValues.Length>0)
            columnValues.Length = columnValues.Length - 1;
            builder.Append(columnNames);
            builder.AppendLine(")");
            builder.AppendLine("Values(");
            builder.Append(columnValues);
            builder.AppendLine(")");
            result = builder.ToString();
            return result;
        }
    }
}
