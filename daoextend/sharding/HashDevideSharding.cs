using daoextend.daoextra;
using daoextend.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace daoextend.sharding
{
    public class HashDevideSharding : ISharding
    {
        public string SharingByObject(List<object> objects, int shardingTotal)
        {
            int hashCode = 0;
            string result = string.Empty;
            object obj = objects?.FirstOrDefault();
            if (obj == null || shardingTotal <= 0) return result;
            hashCode = obj.ToString().HashCode();
            int abs = Math.Abs(hashCode);
            result = (hashCode/shardingTotal+1).ToString();
            return result;
        }
    }
}
