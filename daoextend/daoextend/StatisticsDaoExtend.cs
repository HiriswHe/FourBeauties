using daoextend.attributes;
using daoextend.consts;
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
    public static class StatisticsDaoExtend
    {
        public static List<T> StatisticsByKey<T>(this IStatisticsProperties statisticsProperties, int id = MatchedID.Statistics, string tableIndex = "", List<List<object>> listsIn = null, string sqlAppend = "")
        {
            try
            {
                if (statisticsProperties == null) return default(List<T>);
                string connectionKey = statisticsProperties.GetConnectionKey();
                string connectionString = AppSetting.GetConfig(connectionKey);
                using (IDbConnection dbConnection = statisticsProperties.GetDBConnection(id))
                {
                    dbConnection.Open();
                    var sql = statisticsProperties.GetInSelectSql(id,tableIndex, listsIn, sqlAppend);
                    return dbConnection.Query<T>(sql, statisticsProperties).ToList();
                }
            }
            catch (Exception ex)
            { throw ex; }
        }
                
        public static string GetInSelectSql(this IStatisticsProperties statisticsProperties, int id = MatchedID.Statistics, string tableIndex = "", List<List<object>> listsIn = null, string sqlAppend = "")
        {
            string result = string.Empty;
            StringBuilder builder = new StringBuilder();
            var headersAndFooters = GetStatisticHeaderAndFooterSql(statisticsProperties, id);
            string header = string.Empty;string footer = string.Empty;
            if (headersAndFooters.ContainsKey(StatisticsType.Header))
                header = headersAndFooters[StatisticsType.Header];
            if (headersAndFooters.ContainsKey(StatisticsType.Footer))
                footer= headersAndFooters[StatisticsType.Footer];
            var tableName = statisticsProperties.GetTableName(tableIndex);
            builder.AppendLine(string.Format("Select {0} From {1}", header,tableName));
            var matchedKeys = statisticsProperties.GetInMatchedKeyNameAndValues(id,false, listsIn?.ToArray());
            builder.AppendJoin(" ", matchedKeys);
            builder.AppendLine(sqlAppend);
            builder.AppendLine(footer);
            result = builder.ToString();
            return result;
        }


        public static Dictionary<StatisticsType, string> GetStatisticHeaderAndFooterSql(this IStatisticsProperties statisticsProperties, int id = MatchedID.Statistics)
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

        public static Dictionary<StatisticsType, Dictionary<string, List<Tuple<string, string>>>> GetHeadersAndFooters(this IStatisticsProperties statisticsProperties, int id = MatchedID.Statistics)
        {
            Dictionary<StatisticsType, Dictionary<string, List<Tuple<string, string>>>> result = new Dictionary<StatisticsType, Dictionary<string, List<Tuple<string, string>>>>();
            var propertiesAll = statisticsProperties.GetType().GetProperties();
            if (propertiesAll != null)
                foreach (var property in propertiesAll)
                {
                    if (property == null) return result;
                    var statisticsAll = property.GetCustomAttributes(typeof(StatisticsAttribute),true)?.Select(w=>(StatisticsAttribute)w).Where(w => w.ID == id);
                    foreach (var statistics in statisticsAll)
                        if (statistics != null)
                        {
                            StatisticsType statisticsType = statistics.SType;
                            string column = statistics.Column;
                            string command = statistics.Command;
                            if (!result.ContainsKey(statisticsType)) result.Add(statisticsType, new Dictionary<string, List<Tuple<string, string>>>());
                            if (result.ContainsKey(statisticsType))
                            {
                                var valueKey = result[statisticsType];
                                var columnAliasName = property.Name;
                                if (string.IsNullOrEmpty(column))
                                    column = property.Name;
                                if (valueKey == null || !valueKey.ContainsKey(command))
                                {
                                    result[statisticsType].Add(command, new List<Tuple<string, string>> { new Tuple<string, string>(column, columnAliasName) });
                                }
                                else
                                {
                                    result[statisticsType][command].Add(new Tuple<string, string>(column, columnAliasName));
                                }
                            }
                        }
                }
            return result;
        }

        public static string GetHeaders(Dictionary<StatisticsType, Dictionary<string, List<Tuple<string, string>>>> dicStatisticsTypeAndCommands)
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
                        result.AppendFormat(" {0}{1}{2}{3} As {4} ,", key, "(", value.Item1, ")",value.Item2);
                }
            }
            result.Length = result.Length - 1;
            return result.ToString();
        }

        public static string GetFooters(Dictionary<StatisticsType, Dictionary<string, List<Tuple<string, string>>>> dicStatisticsTypeAndCommands)
        {
            StringBuilder result = new StringBuilder();
            if (dicStatisticsTypeAndCommands == null || !dicStatisticsTypeAndCommands.ContainsKey(StatisticsType.Footer)) return result.ToString();
            var keyValuePairs = dicStatisticsTypeAndCommands[StatisticsType.Footer];
            keyValuePairs = keyValuePairs.OrderBy(w => w.Key).ToDictionary(w=>w.Key,w=>w.Value);
            if (keyValuePairs == null || keyValuePairs.Count == 0) return result.ToString();
            foreach (var key in keyValuePairs.Keys)
            {
                var values = keyValuePairs[key];
                if (values != null)
                {
                    result.AppendFormat("{0} {1} ", key, string.Join(",", values.Select(w=>w.Item1)));
                }
            }
            return result.ToString();
        }
    }
}
