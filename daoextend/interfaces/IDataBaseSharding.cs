using System;
using System.Collections.Generic;
using System.Text;

namespace daoextend.interfaces
{
    public interface IDataBaseSharding: ICURDProperties
    {
        string __DataBaseIndex__ { get; set; }
    }
}
