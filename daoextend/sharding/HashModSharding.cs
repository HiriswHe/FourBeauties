using daoextend.daoextra;
using daoextend.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace daoextend.sharding
{
    public class HashModSharding : ISharding
    {
        public string SharingByObject(List<object> objects, int shardingTotal)
        {
            int hashCode = 0;
            string result = string.Empty;
            object obj = objects?.FirstOrDefault();
            if (obj==null||shardingTotal<=0) return result;
            hashCode = obj.ToString().HashCode();
            var abs = Math.Abs(hashCode);
            result = (abs % shardingTotal+1).ToString();
            return result;
        }
    }
}
