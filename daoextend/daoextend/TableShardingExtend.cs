using daoextend.attributes;
using daoextend.consts;
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
            tableSharding.__TableIndex__ = sharding.SharingByObject(shardingValues, shardingTotal);
        }

        public static List<Object> GetShardingColumnValues(this ITableSharding tableSharding, int id = MatchedID.All)
        {
            var properties = tableSharding.GetType().GetProperties();
            List<object> shardingColumnObjects = new List<object>();
            foreach (var property in properties)
            {
                var shardingColumns = property.GetCustomAttributes(typeof(ShardingColumnAttribute), true)?.Select(w => (ShardingColumnAttribute)w)?.ToList();
                var shardingColumn = shardingColumns.FindAll(w => w.ID == id);
                if (shardingColumn != null && shardingColumn.Count > 0)
                    shardingColumnObjects.Add(property.GetValue(tableSharding, null));
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
