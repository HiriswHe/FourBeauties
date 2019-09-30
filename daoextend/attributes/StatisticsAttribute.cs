using daoextend.consts;
using daoextend.enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace daoextend.attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class StatisticsAttribute:Attribute
    {
        public int ID { get; set; } = MatchedID.Statistics;
        public StatisticsType SType { get; set; } = StatisticsType.Header;
        public string Command { get; set; } = consts.Command.Count;
        public string Column { get; set; }
    }
}
