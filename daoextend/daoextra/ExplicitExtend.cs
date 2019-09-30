using daoextend.attributes;
using daoextend.consts;
using daoextend.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace daoextend.daoextend
{
    public static class ExplicitExtend
    {
        public static T ExplicitToType<T>(this object exclipt,int id=0)
        {
            T result = Activator.CreateInstance<T>();
            if (exclipt == null) return result;
            var type = exclipt.GetType();
            var properties = type.GetProperties();
            if (properties != null)
                foreach (var property in properties)
                {
                    if (property == null) continue;
                    if (property.IgnoreExplicit(exclipt)) continue;
                    PropertyInfo propertyInfoToType = result.GetType().GetProperty(property.Name);
                    if (propertyInfoToType == null)
                    {
                        string toTypePropertyName = property.GetExplicitAliasName(id);
                        propertyInfoToType = result.GetType().GetProperty(toTypePropertyName);
                    }
                    propertyInfoToType?.SetValue(result, property.GetValue(exclipt, null));
                }
            return result;
        }

        public static bool IgnoreExplicit(this PropertyInfo property, object obj, int id = 0)
        {
            if (property == null) return true;
            var ignore = property.GetCustomAttributes(typeof(IgnoreExplicitAttribute), true)?.Select(w => (IgnoreExplicitAttribute)w)?.ToList();
            if (ignore != null && ignore.Count > 0 && ignore.Find(w => w.ID == id || w.ID == MatchedID.All) != null) return true;            
            return false;
        }

        public static string GetExplicitAliasName(this PropertyInfo property, int id = 0)
        {
            string result = string.Empty;
            if (property == null) return result;
            var aliasName = property.GetCustomAttributes(typeof(ExplicitAliasNameAttribute), true)?.Select(w => (ExplicitAliasNameAttribute)w).FirstOrDefault(w => w.ID == 0);
            result = aliasName?.Name;
            if (string.IsNullOrEmpty(result))
                result = property.Name;
            return result;
        }
    }
}
