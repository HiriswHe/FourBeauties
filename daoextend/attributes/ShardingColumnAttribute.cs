using daoextend.consts;
using System;
using System.Collections.Generic;
using System.Text;

namespace daoextend.attributes
{
    [AttributeUsage( AttributeTargets.Property)]
    public class ShardingColumnAttribute:Attribute
    {
        public int ID { get; set; } = MatchedID.All;
        public string Key { get; set; } = "HashKey";
    }
}
