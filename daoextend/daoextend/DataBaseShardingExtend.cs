using daoextend.attributes;
using daoextend.consts;
using daoextend.entity;
using daoextend.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace daoextend.daoextend
{
    public static class DataBaseShardingExtend
    {
        public static void DataBaseSharding(this IDataBaseSharding dataBaseSharding, int id = MatchedID.All, string tableIndex = null)
        {
            if (dataBaseSharding == null) return;
            var shardingValues = dataBaseSharding.GetShardingColumnValues();
            var fatabaseShardingAttribute = dataBaseSharding.GetDataBaseShardingAttribute(id);
            if (fatabaseShardingAttribute == null) return;
            var algorithm = fatabaseShardingAttribute.ShardingAlgorithm;
            var shardingTotal = fatabaseShardingAttribute.ShardingMaxCount;
            ISharding sharding = Activator.CreateInstance(Type.GetType(algorithm)) as ISharding;
            dataBaseSharding.__DataBaseIndex__ = sharding.SharingByObject(shardingValues, shardingTotal);
        }

        public static List<ShardingParameter> GetShardingColumnValues(this IDataBaseSharding dataBaseSharding, int id = MatchedID.All)
        {
            var properties = dataBaseSharding.GetType().GetProperties();
            List<ShardingParameter> shardingColumnObjects = new List<ShardingParameter>();
            foreach (var property in properties)
            {
                var shardingColumns = property.GetCustomAttributes(typeof(ShardingColumnAttribute), true)?.Select(w => (ShardingColumnAttribute)w)?.ToList();
                var shardingColumn = shardingColumns.FindAll(w => w.ID == id).FirstOrDefault();
                if (shardingColumn != null )
                    shardingColumnObjects.Add(new ShardingParameter { Key = shardingColumn.Key, Value = property.GetValue(dataBaseSharding, null) });
            }
            return shardingColumnObjects;
        }

        public static string GetShardingAlgorithm(this IDataBaseSharding dataBaseSharding, int id = MatchedID.All)
        {
            string result = string.Empty;
            var databaseShardingAttribute = GetDataBaseShardingAttribute(dataBaseSharding, id);
            if (databaseShardingAttribute == null) return result;
            var algorithm = databaseShardingAttribute.ShardingAlgorithm;
            return algorithm;
        }

        public static DataBaseShardingAttribute GetDataBaseShardingAttribute(this IDataBaseSharding dataBaseSharding, int id = MatchedID.All)
        {
            var databaseShardingAttribute = dataBaseSharding.GetCustomerAttributes<DataBaseShardingAttribute>().FirstOrDefault(w => w.ID == id);
            return databaseShardingAttribute;
        }
    }
}
