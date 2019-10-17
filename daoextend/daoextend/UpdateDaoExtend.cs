using daoextend.attributes;
using daoextend.consts;
using daoextend.daoextra;
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
        public static bool UpdatePropertiesByKey(this IUpdateProperties updateProperties,int id=0, string tableIndex = null, List<List<object>> listsIn = null, params string[] properties)
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


        public static string GetUpdateSql(this IUpdateProperties updateProperties, int id = 0, string tableIndex = null, List<List<object>> listsIn = null, params string[] properties)
        {
            StringBuilder builder = new StringBuilder();
            var tableName = updateProperties.GetTableName(tableIndex);
            builder.AppendLine(string.Format("Update {0} Set ", tableName));
            if (properties != null && properties.Length > 0)
                foreach (var property in properties)
                {
                    if (property == null) continue;//Ignore Framework Settings Properties
                    if (property.Contains("="))
                        builder.Append(property+" ");
                    else
                        builder.Append(string.Format("{0} = @{0},", property));
                }
            else
            {
                var propertiesAll = updateProperties.GetType().GetProperties();
                if (propertiesAll != null)
                    foreach (var property in propertiesAll)
                    {
                        if (property.IgnoreProperty(updateProperties, id)||property.MatchedProperty(updateProperties,id)!=null//Ignore Matched Column In Where
                            ||property.IsPropertyUsedByFrameWork()) continue;
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
