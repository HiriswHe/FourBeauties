using daoextend.entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace daoextend.interfaces
{
    public interface ISharding
    {
        string SharingByObject(List<ShardingParameter> objects,int shardingTotal);//Support Multiple Columns For Sharding
    }
}
