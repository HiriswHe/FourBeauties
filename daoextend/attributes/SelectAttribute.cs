using System;
using System.Collections.Generic;
using System.Text;

namespace daoextend.attributes
{
    [AttributeUsage(AttributeTargets.Property|AttributeTargets.Class, AllowMultiple = true)]
    public class SelectAttribute:Attribute
    {
        public int ID { get; set; }
        public string CollectionLimit { get; set; } = "";
    }
}
