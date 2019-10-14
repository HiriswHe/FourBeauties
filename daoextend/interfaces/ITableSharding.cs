using System;
using System.Collections.Generic;
using System.Text;

namespace daoextend.interfaces
{
    public interface ITableSharding
    {
        string __TableIndex__ { get; set; }
    }
}
