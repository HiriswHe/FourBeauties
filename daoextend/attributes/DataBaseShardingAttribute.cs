using daoextend.consts;
using System;
using System.Collections.Generic;
using System.Text;

namespace daoextend.attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class DataBaseShardingAttribute : Attribute
    {
        public int ID { get; set; } = MatchedID.All;
        public string ShardingAlgorithm { get; set; }
        public int ShardingMaxCount { get; set; }
        public string DataBaseShardingBegin { get; set; }
        public string DataBaseShardingEnd { get; set; }
    }
}
