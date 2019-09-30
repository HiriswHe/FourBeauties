using System;
using System.Collections.Generic;
using System.Text;

namespace daoextend.interfaces
{
    public interface IInsertProperties: ICURDProperties
    {
        string UUID { get; set; }
    }
}
