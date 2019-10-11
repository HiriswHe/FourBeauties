using System;
using System.Collections.Generic;
using System.Text;

namespace daoextend.daoextra
{
    public static class HashCodeExtend
    {
        public static int HashCode(this string value)
        {
            int hash = 0;
            if (value == null) return hash;
            int h = hash;
            if (h == 0 && value.Length > 0)
            {
                char[] val = value.ToCharArray();

                for (int i = 0; i < value.Length; i++)
                {
                    h = 31 * h + val[i];
                }
                hash = h;
            }
            return h;
        }
    }
}
