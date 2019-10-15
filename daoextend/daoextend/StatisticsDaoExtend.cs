using daoextend.attributes;
using daoextend.consts;
using daoextend.daoextra;
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
        public static List<T> StatisticsByKey<T>(this IStatisticsProperties statisticsProperties, int id = MatchedID.Statistics, string tableIndex = null, List<List<object>> listsIn = null, string sqlAppend = "")
        {
            try
            {
                if (statisticsProperties == null) return default(List<T>);
                using (IDbConnection dbConnection = statisticsProperties.GetDBConnection(id))
                {
                    dbConnection.Open();
                    var sql = statisticsProperties.GetInSelectSql(id,tableIndex, listsIn, sqlAppend);
                    var value = dbConnection.Query<T>(sql, statisticsProperties).ToList();
                    return value;
                }
            }
            catch (Exception ex)
            { throw ex; }
        }
                
        public static string GetInSelectSql(this IStatisticsProperties statisticsProperties, int id = MatchedID.Statistics, string tableIndex = null, List<List<object>> listsIn = null, string sqlAppend = "")
        {
            string result = string.Empty;
            StringBuilder builder = new StringBuilder();
            var headersAndFooters = statisticsProperties.GetStatisticHeaderAndFooterSql(id);
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


        
    }
}
