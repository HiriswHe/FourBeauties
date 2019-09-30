using System;
using System.Collections.Generic;
using System.Text;

namespace daoextend.attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class MatchedKeysAttribute:Attribute
    {
        public int ID { get; set; }
    }
}
