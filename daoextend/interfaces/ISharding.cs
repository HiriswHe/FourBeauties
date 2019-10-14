using System;
using System.Collections.Generic;
using System.Text;

namespace daoextend.interfaces
{
    public interface ISharding
    {
        string SharingByObject(List<object> objects,int shardingTotal);//Support Multiple Columns For Sharding
    }
}
