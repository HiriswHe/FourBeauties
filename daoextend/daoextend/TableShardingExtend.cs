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
    public static class TableShardingExtend
    {
        public static void TableSharding(this ITableSharding tableSharding,int id=MatchedID.All,string tableIndex=null)
        {
            if (tableSharding == null) return;
            var shardingValues = tableSharding.GetShardingColumnValues();
            var tableShardingAttribute = tableSharding.GetTableShardingAttribute(id);
            if (tableShardingAttribute == null) return;
            var algorithm = tableShardingAttribute.ShardingAlgorithm;
            var shardingTotal = tableShardingAttribute.ShardingTotalCount;
            ISharding sharding = Activator.CreateInstance(Type.GetType(algorithm)) as ISharding;
            var tableIndexSharding = sharding.SharingByObject(shardingValues, shardingTotal);
            if (!string.IsNullOrEmpty(tableIndexSharding))
                tableSharding.__TableIndex__ = tableIndexSharding;
        }

        public static List<ShardingParameter> GetShardingColumnValues(this ITableSharding tableSharding, int id = MatchedID.All)
        {
            var properties = tableSharding.GetType().GetProperties();
            List<ShardingParameter> shardingColumnObjects = new List<ShardingParameter>();
            foreach (var property in properties)
            {
                var shardingColumns = property.GetCustomAttributes(typeof(ShardingColumnAttribute), true)?.Select(w => (ShardingColumnAttribute)w)?.ToList();
                var shardingColumn = shardingColumns.FindAll(w => w.ID == id).FirstOrDefault();
                if (shardingColumn != null )
                    shardingColumnObjects.Add(new ShardingParameter { Key = shardingColumn.Key, Value = property.GetValue(tableSharding, null) });
            }
            return shardingColumnObjects;
        }

        public static string GetShardingAlgorithm(this ITableSharding tableSharding, int id = MatchedID.All)
        {
            string result = string.Empty;
            var tableShardingAttribute = GetTableShardingAttribute(tableSharding, id);
            if (tableShardingAttribute == null) return result;
            var algorithm = tableShardingAttribute.ShardingAlgorithm;
            return algorithm;
        }

        public static TableShardingAttribute GetTableShardingAttribute(this ITableSharding tableSharding, int id = MatchedID.All)
        {
            var tableShardingAttribute = tableSharding.GetCustomerAttributes<TableShardingAttribute>().FirstOrDefault(w => w.ID == id);
            return tableShardingAttribute;
        }
    }
}
