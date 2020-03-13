using daoextend.attributes;
using daoextend.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace daoextend.daoextend
{
    public static class WhereExtend
    {

        public static string GetPropertyAliasName(this PropertyInfo property, int id = 0)
        {
            string result = string.Empty;
            if (property == null) return result;
            var matchedColumn = property.GetCustomAttributes(typeof(MatchedColumnAttribute),true).Select(w=>(MatchedColumnAttribute)w)?.FirstOrDefault(w => w.ID == id);
            if (matchedColumn != null)
            {
                result = matchedColumn.AliasName;
            }
            else
            {
                var aliasName = property.GetCustomAttributes(typeof(DaoAliasNameAttribute), true)?.Select(w=> (DaoAliasNameAttribute)w).FirstOrDefault(w => w.ID == 0);
                result = aliasName?.Name;
            }
            if (string.IsNullOrEmpty(result))
                result = property.Name;
            return result;
        }

        public static List<string> GetMatchedKeyNameAndValues(this ICURDProperties updateProperties, int id = 0)
        {
            List<string> result = new List<string>();
            if (updateProperties == null) return result;
            var properties = updateProperties.GetType().GetProperties();
            result.Add(" Where 1=1 ");
            if (properties != null)
                foreach (var property in properties)
                {
                    if (property.IgnoreProperty(updateProperties, id)) continue;
                    var matchedKeyAttribute = property.GetCustomAttributes(typeof(MatchedKeysAttribute), true).Select(w => w as MatchedKeysAttribute).FirstOrDefault(w => w.ID == id);
                    if (matchedKeyAttribute != null)
                    {
                        var key = property.Name;
                        result.Add(string.Format("and {0}=@{0}", key));
                    }
                    var matchedColumnAttribute = property.GetCustomAttributes(typeof(MatchedColumnAttribute), true).Select(w => w as MatchedColumnAttribute).FirstOrDefault(w => w.ID == id);
                    if (matchedColumnAttribute != null)
                    {
                        var columnName = matchedColumnAttribute.AliasName;
                        if (string.IsNullOrEmpty(columnName)) columnName = property.Name;
                        var contactNotation = matchedColumnAttribute.ContactNotation;
                        var columnValue = "@" + property.Name;
                        var filterRelation = matchedColumnAttribute.FilterRelation;
                        if (string.Compare(contactNotation, "like", true) == 0 && property.PropertyType == typeof(string))
                        {
                            property.SetValue(updateProperties, "%" + property.GetValue(updateProperties, null) + "%");
                        }
                        result.Add(string.Format(" {0} {1} {2} {3} ", filterRelation, columnName, contactNotation, columnValue));
                    }

                }
            return result;
        }
        public static List<string> GetInMatchedKeyNameAndValues(this ICURDProperties updateProperties, int id = 0,bool ignoreMatched=false, params List<object>[] listsIn)
        {
            List<string> result = new List<string>();
            if (updateProperties == null) return result;
            var properties = updateProperties.GetType().GetProperties();
            result.Add(" Where 1=0 ");
            int i = 0;
            if (properties != null)
                foreach (var property in properties)
                {
                    if (!ignoreMatched&& property.IgnoreProperty(updateProperties, id)) continue;
                    if (ignoreMatched && property.IgnoreMatchedProperty(updateProperties, id)) continue;
                    var matchedKeyAttribute = property.GetCustomAttributes(typeof(MatchedKeysAttribute), true).Select(w => w as MatchedKeysAttribute).FirstOrDefault(w => w.ID == id);
                    if (matchedKeyAttribute != null)
                    {
                        var key = property.Name;
                        result.Add(string.Format("And {0}=@{0}", key));
                    }
                    var matchedColumnAttribute = property.GetCustomAttributes(typeof(MatchedColumnAttribute), true).Select(w => w as MatchedColumnAttribute).FirstOrDefault(w => w.ID == id);
                    if (matchedColumnAttribute != null)
                    {
                        var columnName = matchedColumnAttribute.AliasName;
                        if (string.IsNullOrEmpty(columnName)) columnName = property.Name;
                        var contactNotation = matchedColumnAttribute.ContactNotation;
                        var columnValue = "@" + property.Name;
                        var filterRelation = matchedColumnAttribute.FilterRelation;
                        if (string.Compare(contactNotation, "like", true) == 0 && property.PropertyType == typeof(string))
                        {
                            property.SetValue(updateProperties, "%" + property.GetValue(updateProperties, null) + "%");
                        }
                        if (!string.IsNullOrEmpty(contactNotation) && contactNotation.ToUpper().Contains("IN") && listsIn != null && listsIn.Length > i)
                        {
                            columnValue = property.GetColumnInValue(i, listsIn);
                            result.Add(string.Format(" {0} {1} {2} {3} ", filterRelation, columnName, contactNotation, columnValue));
                            i++;
                        }
                        else
                        {
                            if (property.GetValue(updateProperties, null) == null && contactNotation == "=") { contactNotation = "is"; columnValue = "null"; }
                            result.Add(string.Format(" {0} {1} {2} {3} ", filterRelation, columnName, contactNotation, columnValue));
                        }
                    }

                }
            if (result.Count > 1&&!string.IsNullOrEmpty(result[1]))
            {
                result[1]=result[1].Replace("And", "Or ( ");
                result[result.Count - 1] += " ) ";
            }
            return result;
        }

        public static string GetColumnInValue(this PropertyInfo property, int i, params List<object>[] listsIn)
        {
            string columnValue = string.Empty;
            var instance = listsIn[i];
            if (instance == null || instance.Count == 0) return columnValue;
            var columnName = property.GetPropertyAliasName();
            var propertyInstance = instance.FirstOrDefault().GetType().GetProperty(columnName);
            var listValues = new List<object>();
            if (propertyInstance == null)
            {
                listValues = instance;
            }
            else
            {
                listValues = instance.Select(w =>
                {
                    if (w == null) return null;
                    var type = w.GetType();
                    var propertyInfo = type.GetProperty(columnName);
                    return property?.GetValue(w);
                }).ToList();
            }
            if (property.PropertyType == typeof(string) || property.PropertyType == typeof(DateTime))
            {
                columnValue = string.Format("('{0}')", string.Join("','", listValues.Select(w => w?.ToString())));
            }
            else
            {
                columnValue = string.Format("({0})", string.Join(",", listValues.Select(w => w?.ToString())));
            }
            return columnValue;
        }

    }
}
