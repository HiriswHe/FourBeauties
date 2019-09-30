using daoextend.enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace daoextend.attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class MatchedTableAttribute:Attribute
    {
        public DBServerType DBServerType { get; set; } = DBServerType.SqlServer;
        public int ID { get; set; }
        public string Name { get; set; }
        public string ConnectionKey { get; set; }

        public MatchedTableAttribute()
        { }
        public MatchedTableAttribute(string name)
        {
            Name = name;
        }

        public MatchedTableAttribute(string name,string connectionKey)
        {
            Name = name;
            ConnectionKey = connectionKey;
        }
        public MatchedTableAttribute(int id,string name, string connectionKey)
        {
            ID = id;
            Name = name;
            ConnectionKey = connectionKey;
        }
    }
}
