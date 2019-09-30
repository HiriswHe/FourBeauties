using System;
using System.Collections.Generic;
using System.Text;

namespace daoextend.attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class IgnoreAttribute:Attribute
    {
        public int ID { get; set; }
    }

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class IgnoreSelectAttribute : Attribute
    {
        public int ID { get; set; }
    }

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class IgnoreExplicitAttribute : Attribute
    {
        public int ID { get; set; }
    }
}
