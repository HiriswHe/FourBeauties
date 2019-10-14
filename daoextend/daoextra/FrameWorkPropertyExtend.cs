using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace daoextend.daoextra
{
    public static class FrameWorkPropertyExtend
    {

        public static bool IsPropertyUsedByFrameWork(this PropertyInfo property)
        {
            bool result = false;
            if (property == null) return result;
            result = property.Name.StartsWith("__") && property.Name.EndsWith("__");
            return result;
        }
    }
}
