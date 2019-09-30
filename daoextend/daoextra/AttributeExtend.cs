using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace daoextend.daoextend
{
    public static class AttributeExtend
    {
        public static List<TAttribute> GetCustomerAttributes<TAttribute>(this object objInstance, TAttribute attribute = null) where TAttribute : Attribute
        {
            List<TAttribute> result = new List<TAttribute>();
            if (object.Equals(objInstance, null)) return result;
            object[] attrs = objInstance.GetType().GetCustomAttributes(typeof(TAttribute), true);
            if (attrs != null && attrs.Length > 0)
            {
                return attrs.Select(w => w as TAttribute).ToList();
            }
            return result;
        }
    }
}
