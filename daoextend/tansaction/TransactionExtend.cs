using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using daoextend.consts;
using daoextend.interfaces;
using Dapper;
using daoextend.daoextend;
using System.Linq;
using daoextend.log;

namespace daoextend.tansaction
{
    public static class TransactionExtend
    {
        public static bool ExecuteTransaction(this IDictionary<ICURDAll,int> dicPosAndID)
        {
            bool result = false;
            if (dicPosAndID == null || dicPosAndID.Count == 0) return result;
            try
            {
                var dic = dicPosAndID.GetKeyValuePairs();
                var first = dicPosAndID.Keys.FirstOrDefault();
                IDbConnection dbConnection = first.GetDBConnection(dicPosAndID[first]);
                ExecuteTransactionBySql(dbConnection, dic, ExecuteDictionary);
                result = true;
            }
            catch (Exception ex)
            {
                throw ex;
                //Logger.Error("ExecuteTransaction Error:" + ex.ToString());
            }
            return result;
        }

        public static Dictionary<string, List<object>> GetKeyValuePairs(this IDictionary<ICURDAll,int> dicPosAndID)
        {
            Dictionary<string,List<object>> result = new Dictionary<string,List<object>>();
            if (dicPosAndID == null || dicPosAndID.Count == 0) return result;
            foreach (var key in dicPosAndID.Keys)
            {
                if (key == null) continue;
                var value = dicPosAndID[key];
                string sql = string.Empty;
                switch (value)
                {
                    case MatchedID.Insert:
                        sql = key.GetInsertSql(value);
                        break;
                    case MatchedID.Delete:
                        sql = key.GetDeleteSql(value);
                        break;
                    case MatchedID.Update:
                        sql = key.GetUpdateSql(value);
                        break;
                    case MatchedID.SelectAll:
                        sql = key.GetSelectSql(value);
                        break;
                    default:
                        sql = key.GetInsertSql(value);
                        break;
                }
                if (!result.Keys.Contains(sql))
                    result.Add(sql, new List<object> { key });
                else
                {
                    if (result[sql] == null)
                        result[sql] = new List<object>();
                    result[sql].Add(key);
                }
            }
            return result;
        }

        public static void ExecuteDictionary(Dictionary<string, List<object>> keyValuePairs, ConnectionProvider provider)
        {
            if (keyValuePairs == null || keyValuePairs.Count == 0 || provider == null) return;
            foreach (var key in keyValuePairs.Keys)
            {
                string exeSql = key;
                if (string.IsNullOrEmpty(exeSql)) continue;
                List<object> list = keyValuePairs[key];
                if (list == null || list.Count == 0) continue;
                for (int i = 0; i < list.Count; i++)
                {
                    provider.DBConnection.Execute(exeSql, list[i], provider.CurrentTransaction);
                }
            }
        }

        public static bool ExecuteTransactionBySql(IDbConnection dbConnection, Dictionary<string, List<object>> keyValuePairs, Action<Dictionary<string, List<object>>, ConnectionProvider> actionExecute)
        {
            bool result = false;
            if (keyValuePairs == null || keyValuePairs.Keys.Count == 0) return result;
            using (ConnectionProvider provider = new ConnectionProvider(dbConnection))
            {
                bool connectionOpened = false;
                bool transactionPended = false;
                string connectionString = string.Empty;
                try
                {
                    connectionOpened = provider.OpenConnection();
                    connectionString = provider.DBConnection.ConnectionString;
                    transactionPended = provider.BeginTransaction();
                    actionExecute?.Invoke(keyValuePairs, provider);
                    provider.CommitTransaction();
                    return true;
                }
                catch (System.Exception ex)
                {
                    try
                    {
                        provider.RollbackTransaction();
                    }
                    catch { }
                    var exception = new Exception("Connection Opened:" + connectionOpened.ToString() + Environment.NewLine +
                        "Transaction Pending:" + transactionPended + Environment.NewLine +
                        "DBConnection String:" + connectionString +
                         "Excetption:" + ex.ToString());
                    throw exception;
                }
            }
        }
    }
}
