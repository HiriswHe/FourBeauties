using daoextend.consts;
using System;
using System.Collections.Generic;
using System.Text;

namespace daoextend.attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class TableShardingAttribute : Attribute
    {
        public int ID { get; set; } = MatchedID.All;
        public string ShardingAlgorithm { get; set; }
        public int ShardingTotalCount { get; set; }
        public string TableShardingBegin { get; set; }
        public string TableShardingEnd { get; set; }
    }
}
