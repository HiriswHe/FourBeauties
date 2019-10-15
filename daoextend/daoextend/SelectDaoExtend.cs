using daoextend.attributes;
using daoextend.consts;
using daoextend.daoextra;
using daoextend.entity;
using daoextend.enums;
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

        public static List<T> SelectPropertiesExists<T>(this ISelectProperties selectProperties, int id = MatchedID.SelectExists, string tableIndex = null, List<List<object>> listsIn = null, string sqlAppend = "", params string[] properties)
        {
            try
            {
                if (selectProperties == null) return default(List<T>);
                using (IDbConnection dbConnection = selectProperties.GetDBConnection(id))
                {
                    dbConnection.Open();
                    var sql = selectProperties.GetInSelectSql(id,tableIndex, listsIn, sqlAppend, properties);
                    //Logger.Info(sql);
                    return dbConnection.Query<T>(sql, selectProperties).ToList();
                }
            }
            catch (Exception ex)
            { throw ex; }
        }

        public static List<T> SelectPropertiesByKey<T>(this ISelectProperties selectProperties,int id = 0, string tableIndex = null, List<List<object>> listsIn=null, string sqlAppend="",  params string[] properties)
        {
            try
            {
                if (selectProperties == null) return default(List<T>);
                string connectionKey = selectProperties.GetConnectionKey();
                string connectionString = AppSetting.GetConfig(connectionKey);
                using (IDbConnection dbConnection = selectProperties.GetDBConnection(id))
                {
                    dbConnection.Open();
                    var sql = selectProperties.GetInSelectSql(id,tableIndex,listsIn,sqlAppend, properties);
                    //Logger.Info(sql);
                    //if (listsIn == null)
                        return dbConnection.Query<T>(sql, selectProperties).ToList();
                    //else
                     //   return dbConnection.Query<T>(sql).ToList();
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

        public static string GetSelectSql(this ISelectProperties selectProperties, int id = 0, string tableIndex = null, string sqlAppend="", params string[] properties)
        {
            string result = string.Empty;
            StringBuilder builder = new StringBuilder();
            var selectColumnNames = GetSelectColumnSql(selectProperties,id,properties);
            var tableName = selectProperties.GetTableName(tableIndex);
            var collectionLimit = selectProperties.GetSelectCollectionLimit(id);
            builder.AppendLine(string.Format("Select {0} {1} From {2} ",collectionLimit, selectColumnNames.ToString(), tableName));
            var matchedKeys = selectProperties.GetMatchedKeyNameAndValues(id);
            builder.AppendJoin(" ", matchedKeys);
            builder.AppendLine(sqlAppend);
            result = builder.ToString();
            return result;
        }

        public static string GetInSelectSql(this ISelectProperties selectProperties, int id = 0, string tableIndex = null, List< List<object>> listsIn=null, string sqlAppend = "", params string[] properties)
        {
            string result = string.Empty;
            StringBuilder builder = new StringBuilder();
            var selectColumnNames = GetSelectColumnSql(selectProperties, id, properties);
            var headersAndFooters = selectProperties.GetStatisticHeaderAndFooterSql(id);
            string header = string.Empty; string footer = string.Empty;
            if (headersAndFooters.ContainsKey(StatisticsType.Header))
                header = headersAndFooters[StatisticsType.Header];
            if (headersAndFooters.ContainsKey(StatisticsType.Footer))
                footer = headersAndFooters[StatisticsType.Footer];
            var tableName = selectProperties.GetTableName(tableIndex);
            var collectionLimit = selectProperties.GetSelectCollectionLimit(id);
            builder.AppendLine(string.Format("Select {0} {1} From {2} ", collectionLimit, selectColumnNames.ToString(), tableName));
            var matchedKeys = selectProperties.GetInMatchedKeyNameAndValues(id,true,listsIn?.ToArray());
            builder.AppendJoin(" ", matchedKeys);
            builder.AppendLine(sqlAppend);
            builder.Append(footer);
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
                        if (property.IgnoreSelectProperty(selectProperties, id)||property.IsPropertyUsedByFrameWork()) continue;
                        string columnName = property.GetPropertyAliasName(id);
                        columnNames.Append(columnName + ",");                        
                    }
            }
            if(columnNames.Length>1)
                columnNames.Length = columnNames.Length - 1;
            return columnNames.ToString();
        }

        public static Dictionary<StatisticsType, string> GetStatisticHeaderAndFooterSql(this ISelectProperties statisticsProperties, int id = MatchedID.Statistics)
        {
            Dictionary<StatisticsType, string> result = new Dictionary<StatisticsType, string>();
            string headers = string.Empty;
            string footers = string.Empty;
            var headersAndFooters = statisticsProperties.GetHeadersAndFooters(id);
            if (headersAndFooters == null) return result;
            result.Add(StatisticsType.Header, GetHeaders(headersAndFooters));
            result.Add(StatisticsType.Footer, GetFooters(headersAndFooters));
            return result;
        }

        public static Dictionary<StatisticsType, Dictionary<string, List<StatisticsParameter>>> GetHeadersAndFooters(this ISelectProperties statisticsProperties, int id = MatchedID.Statistics)
        {
            Dictionary<StatisticsType, Dictionary<string, List<StatisticsParameter>>> result = new Dictionary<StatisticsType, Dictionary<string, List<StatisticsParameter>>>();
            var propertiesAll = statisticsProperties.GetType().GetProperties();
            if (propertiesAll != null)
                foreach (var property in propertiesAll)
                {
                    if (property == null || property.IsPropertyUsedByFrameWork()) continue;
                    var statisticsAll = property.GetCustomAttributes(typeof(StatisticsAttribute), true)?.Select(w => (StatisticsAttribute)w).Where(w => w.ID == id);
                    foreach (var statistics in statisticsAll)
                        if (statistics != null)
                        {
                            StatisticsType statisticsType = statistics.SType;
                            string column = statistics.Column;
                            string command = statistics.Command;
                            var extra = statistics.CommandExtra;
                            var sort = statistics.Sort;
                            if (!result.ContainsKey(statisticsType)) result.Add(statisticsType, new Dictionary<string, List<StatisticsParameter>>());
                            if (result.ContainsKey(statisticsType))
                            {
                                var valueKey = result[statisticsType];
                                var columnAliasName = property.Name;                                
                                if (string.IsNullOrEmpty(column))
                                    column = property.Name;
                                if (valueKey == null || !valueKey.ContainsKey(command))
                                {
                                    result[statisticsType].Add(command, new List<StatisticsParameter> { new StatisticsParameter {
                                        CommandText = column+" " + extra,ColumnAliasName= columnAliasName,Sort= sort} });
                                }
                                else
                                {
                                    result[statisticsType][command].Add(new StatisticsParameter
                                    {
                                        CommandText = column + " " + extra,
                                        ColumnAliasName = columnAliasName,
                                        Sort = sort
                                    });
                                }
                            }
                        }
                }
            return result;
        }

        public static string GetHeaders(Dictionary<StatisticsType, Dictionary<string, List<StatisticsParameter>>> dicStatisticsTypeAndCommands)
        {
            StringBuilder result = new StringBuilder();
            if (dicStatisticsTypeAndCommands == null || !dicStatisticsTypeAndCommands.ContainsKey(StatisticsType.Header)) return result.ToString();
            var keyValuePairs = dicStatisticsTypeAndCommands[StatisticsType.Header];
            if (keyValuePairs == null || keyValuePairs.Count == 0) return result.ToString();
            foreach (var key in keyValuePairs.Keys)
            {
                var values = keyValuePairs[key];
                if (values != null)
                {
                    foreach (var value in values)
                        result.AppendFormat(" {0}{1}{2}{3} As {4} ,", key, "(", value.CommandText, ")", value.ColumnAliasName);
                }
            }
            result.Length = result.Length - 1;
            return result.ToString();
        }

        public static string GetFooters(Dictionary<StatisticsType, Dictionary<string, List<StatisticsParameter>>> dicStatisticsTypeAndCommands)
        {
            StringBuilder result = new StringBuilder();
            if (dicStatisticsTypeAndCommands == null || !dicStatisticsTypeAndCommands.ContainsKey(StatisticsType.Footer)) return result.ToString();
            var keyValuePairs = dicStatisticsTypeAndCommands[StatisticsType.Footer];
            keyValuePairs = keyValuePairs.OrderBy(w => w.Key).ToDictionary(w => w.Key, w => w.Value);
            if (keyValuePairs == null || keyValuePairs.Count == 0) return result.ToString();
            foreach (var key in keyValuePairs.Keys)
            {
                var values = keyValuePairs[key];
                if (values != null)
                {
                    result.AppendFormat("{0} {1} ", key, string.Join(",", values.OrderBy(w=>w.Sort).Select(w => w.CommandText)));
                }
            }
            return result.ToString();
        }
    }
}
