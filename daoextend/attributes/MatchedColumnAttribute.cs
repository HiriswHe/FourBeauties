using daoextend.consts;
using System;
using System.Collections.Generic;
using System.Text;

namespace daoextend.attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple =true)]
    public class MatchedColumnAttribute:Attribute
    {
        private int id = 0;
        public int ID
        {
            get { return id; }
            set
            {
                id = value;
                if (id == MatchedID.SelectIn || id == MatchedID.UpdateIn || id == MatchedID.DeleteIn)
                {
                    ContactNotation = "in";
                }
            }
        }
        public string AliasName { get; set; }
        public string ContactNotation { get; set; } = "=";
        public string IgnoreHashCode { get; set; }
        public string FilterRelation { get; set; } = "And";
    }
}
