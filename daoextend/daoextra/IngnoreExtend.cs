using daoextend.attributes;
using daoextend.consts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace daoextend.daoextend
{
    public static class IngnoreExtend
    {
        public static bool IgnoreProperty(this PropertyInfo property, object obj, int id = 0)
        {
            if (property == null) return true;
            var ignore = property.GetCustomAttributes(typeof(IgnoreAttribute), true)?.Select(w => (IgnoreAttribute)w)?.ToList();
            if (ignore != null && ignore.Count > 0 && ignore.Find(w => w.ID == id || w.ID == MatchedID.All) != null) return true;
            return false;
        }

        public static bool IgnoreMatchedProperty(this PropertyInfo property, object obj, int id = 0)
        {
            if (property == null) return true;
            var matchedColumn = property.GetCustomAttributes(typeof(MatchedColumnAttribute), true)?.Select(w => (MatchedColumnAttribute)w)?.FirstOrDefault(w => w.ID == id);
            if (matchedColumn != null)
            {
                var ignoreValue = matchedColumn.IgnoreHashCode;
                var columnValue = property.GetValue(obj, null);
                if (string.IsNullOrEmpty(columnValue?.ToString()) && string.Compare(ignoreValue, "null", true) == 0) return true;
                var columnValueHashCode = columnValue?.GetHashCode().ToString();
                if (columnValueHashCode == ignoreValue && ignoreValue != null) return true;
            }
            else return true;
            return false;
        }

        public static bool IgnoreSelectProperty(this PropertyInfo property, object obj, int id = 0)
        {
            if (property == null) return true;
            var ignore = property.GetCustomAttributes(typeof(IgnoreSelectAttribute), true)?.Select(w => (IgnoreSelectAttribute)w)?.ToList();
            if (ignore != null && ignore.Count > 0 && ignore.Find(w => w.ID == id) != null) return true;
            var ignoreAll = property.GetCustomAttributes(typeof(IgnoreAttribute), true)?.Select(w => (IgnoreAttribute)w)?.ToList();
            if (ignoreAll != null && ignoreAll.Count > 0 && ignoreAll.Find(w => w.ID == MatchedID.All) != null) return true;
            return false;
        }

    }
}
