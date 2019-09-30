using System;
using System.Collections.Generic;
using System.Text;

namespace daoextend.attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public  class DaoAliasNameAttribute:Attribute
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class ExplicitAliasNameAttribute : Attribute
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }
}
