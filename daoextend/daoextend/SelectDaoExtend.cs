using daoextend.attributes;
using daoextend.consts;
using daoextend.interfaces;
using daoextend.log;
using daoextend.utils;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace daoextend.daoextend
{
    public static class SelectDaoExtend
    {
        public static List<T> SelectPropertiesBySql<T>(this ISelectProperties selectProperties, string sql, bool needParameters = false, int id = MatchedID.SelectBySql)
        {
            try
            {
                if (selectProperties == null) return default(List<T>);
                string connectionKey = selectProperties.GetConnectionKey();
                string connectionString = CommonHelpr.GetConfig(connectionKey);
                using (IDbConnection dbConnection = selectProperties.GetDBConnection(id))
                {
                    dbConnection.Open();
                    if (needParameters)
                        return dbConnection.Query<T>(sql, selectProperties).ToList();
                    else
                        return dbConnection.Query<T>(sql).ToList();
                }
            }
            catch (Exception ex)
            { throw ex; }
        }

        public static List<T> SelectPropertiesExists<T>(this ISelectProperties selectProperties, int id = MatchedID.SelectExists, List<List<object>> listsIn = null, string sqlAppend = "", params string[] properties)
        {
            try
            {
                if (selectProperties == null) return default(List<T>);
                string connectionKey = selectProperties.GetConnectionKey();
                string connectionString = CommonHelpr.GetConfig(connectionKey);
                using (IDbConnection dbConnection = selectProperties.GetDBConnection(id))
                {
                    dbConnection.Open();
                    var sql = selectProperties.GetInSelectSql(id, listsIn, sqlAppend, properties);
                    //Logger.Info(sql);
                    return dbConnection.Query<T>(sql, selectProperties).ToList();
                }
            }
            catch (Exception ex)
            { throw ex; }
        }

        public static List<T> SelectPropertiesByKey<T>(this ISelectProperties selectProperties,int id = 0,List<List<object>> listsIn=null, string sqlAppend="",  params string[] properties)
        {
            try
            {
                if (selectProperties == null) return default(List<T>);
                string connectionKey = selectProperties.GetConnectionKey();
                string connectionString = CommonHelpr.GetConfig(connectionKey);
                using (IDbConnection dbConnection = selectProperties.GetDBConnection(id))
                {
                    dbConnection.Open();
                    var sql = selectProperties.GetInSelectSql(id,listsIn,sqlAppend, properties);
                    //Logger.Info(sql);
                    return dbConnection.Query<T>(sql, selectProperties).ToList();
                }
            }
            catch (Exception ex)
            { throw ex; }
        }
        

        public static string GetSelectCollectionLimit(this ISelectProperties selectProperties, int id = 0)
        {
            string result = string.Empty;
            if (selectProperties == null) return result;
            var selectAttribute = selectProperties.GetCustomerAttributes<SelectAttribute>()?.Find(w => w.ID == id);
            if (selectAttribute != null)
            {
                result = selectAttribute.CollectionLimit;
            }
            return result;
        }

        public static string GetSelectSql(this ISelectProperties selectProperties, int id = 0,string sqlAppend="", params string[] properties)
        {
            string result = string.Empty;
            StringBuilder builder = new StringBuilder();
            var selectColumnNames = GetSelectColumnSql(selectProperties,id,properties);
            var tableName = selectProperties.GetTableName();
            var collectionLimit = selectProperties.GetSelectCollectionLimit(id);
            builder.AppendLine(string.Format("Select {0} {1} From {2} ",collectionLimit, selectColumnNames.ToString(), tableName));
            var matchedKeys = selectProperties.GetMatchedKeyNameAndValues(id);
            builder.AppendJoin(" ", matchedKeys);
            builder.AppendLine(sqlAppend);
            result = builder.ToString();
            return result;
        }

        public static string GetInSelectSql(this ISelectProperties selectProperties, int id = 0,  List< List<object>> listsIn=null, string sqlAppend = "", params string[] properties)
        {
            string result = string.Empty;
            StringBuilder builder = new StringBuilder();
            var selectColumnNames = GetSelectColumnSql(selectProperties, id, properties);
            var tableName = selectProperties.GetTableName();
            var collectionLimit = selectProperties.GetSelectCollectionLimit(id);
            builder.AppendLine(string.Format("Select {0} {1} From {2} ", collectionLimit, selectColumnNames.ToString(), tableName));
            var matchedKeys = selectProperties.GetInMatchedKeyNameAndValues(id,true,listsIn?.ToArray());
            builder.AppendJoin(" ", matchedKeys);
            builder.AppendLine(sqlAppend);
            result = builder.ToString();
            return result;
        }


        public static string GetSelectColumnSql(this ISelectProperties selectProperties, int id = 0, params string[] properties)
        {
            StringBuilder columnNames = new StringBuilder();
            if (properties != null && properties.Length > 0)
                foreach (var property in properties)
                {
                    columnNames.Append(property + ",");                    
                }
            else
            {
                var propertiesAll = selectProperties.GetType().GetProperties();
                if (propertiesAll != null)
                    foreach (var property in propertiesAll)
                    {
                        if (property.IgnoreSelectProperty(selectProperties, id)) continue;
                        string columnName = property.GetPropertyAliasName(id);
                        columnNames.Append(columnName + ",");                        
                    }
            }
            if(columnNames.Length>1)
                columnNames.Length = columnNames.Length - 1;
            return columnNames.ToString();
        }
    }
}
