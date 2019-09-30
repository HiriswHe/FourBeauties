using daoextend.attributes;
using daoextend.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace daoextend.daoextend
{
    public static class TableExtend
    {

        public static string GetTableName(this ICURDProperties updateProperties, int id = 0)
        {
            var tableAttribute = updateProperties.GetCustomerAttributes<MatchedTableAttribute>().FirstOrDefault(w => w.ID == id);
            var tableName = tableAttribute?.Name;
            if (string.IsNullOrEmpty(tableName))
            {
                tableName = updateProperties.GetType().Name;
            }
            return tableName;
        }


        public static MatchedTableAttribute GetMatchedTableAttribute(this ICURDProperties updateProperties, int id = 0)
        {
            var tableAttribute = updateProperties.GetCustomerAttributes<MatchedTableAttribute>().FirstOrDefault(w => w.ID == id);
            return tableAttribute;
        }

        public static string GetConnectionKey(this ICURDProperties updateProperties, int id = 0)
        {
            var tableAttribute = updateProperties.GetCustomerAttributes<MatchedTableAttribute>().FirstOrDefault(w => w.ID == id);
            var connectionKey = tableAttribute?.ConnectionKey;
            return connectionKey;
        }

    }
}
