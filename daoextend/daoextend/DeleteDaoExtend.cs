using daoextend.daoextra;
using daoextend.interfaces;
using daoextend.utils;
using Dapper;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace daoextend.daoextend
{
    public static class DeleteDaoExtend
    {
        public static bool DeletePropertiesByKey(this IDeleteProperties deleteProperties, int id = 0, string tableIndex = null, List<List<object>> listsIn = null, string sqlAppend = "")
        {
            try
            {
                if (deleteProperties == null) return false;
                using (IDbConnection dbConnection = deleteProperties.GetDBConnection(id))
                {
                    dbConnection.Open();
                    var sql = deleteProperties.GetDeleteSql(id,tableIndex,listsIn);
                    var result = dbConnection.Execute(sql, deleteProperties) > 0;
                    if (!result) throw new Exception("没有匹配的记录");
                    return result;
                }
            }
            catch (Exception ex)
            { throw ex; }
        }

        public static string GetDeleteSql(this IDeleteProperties deleteProperties, int id = 0, string tableIndex = null, List<List<object>> listsIn = null, string sqlAppend = "")
        {
            string result = string.Empty;
            StringBuilder builder = new StringBuilder();
            var tableName = deleteProperties.GetTableName(tableIndex);
            builder.AppendLine(string.Format("Delete From {0} ", tableName));
            var matchedKeys = deleteProperties.GetInMatchedKeyNameAndValues(id,false,listsIn?.ToArray());
            builder.AppendJoin(" ", matchedKeys);
            builder.Append(sqlAppend);
            result = builder.ToString();
            return result;
        }
    }
}
