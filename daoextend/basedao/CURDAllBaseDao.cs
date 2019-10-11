using daoextend.consts;
using daoextend.daoextend;
using daoextend.enums;
using daoextend.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace daoextend.basedao
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class CURDAllBaseDao<T> where T:ICURDAll
    {

        public virtual void Insert(T po,int id=MatchedID.Insert, string tableIndex = "")
        {
            try
            {
                if (string.IsNullOrEmpty(po?.UUID)) po.UUID = Guid.NewGuid().ToString("N");
                po.InsertProperties(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public virtual void DeleteByKey(T po,int id=MatchedID.Delete, string tableIndex ="")
        {
            try
            {
                po.DeletePropertiesByKey(id,tableIndex);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public virtual void UpdateByKey(T po,int id=MatchedID.Update, string tableIndex = "", params string[] parameters)
        {
            try
            {
                po.UpdatePropertiesByKey(id,tableIndex,null,parameters);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public virtual List<TDTO> SelectAllByKey<TDTO>(T po,int id=MatchedID.SelectAll,string tableIndex = "")
        {
            try
            {
                return po.SelectPropertiesByKey<TDTO>(id,tableIndex);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public virtual TDTO SelectExists<TDTO>(T po, int id = MatchedID.SelectExists, string tableIndex = "")
        {
            try
            {
                return po.SelectPropertiesExists<TDTO>(id,tableIndex).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public virtual TDTO SelectSingleByKey<TDTO>(T po,int id=MatchedID.SelectSingle, string tableIndex = "")
        {
            try
            {
                return po.SelectPropertiesByKey<TDTO>(MatchedID.SelectSingle,tableIndex).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        public virtual List<TDTO> SelectByIn<TDTO>(T po, int id = MatchedID.SelectIn, string tableIndex = "", params List<object>[] listsIn)
        {
            try
            {
                return po.SelectPropertiesByKey<TDTO>(id,tableIndex, listsIn?.ToList());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public virtual bool DeleteByIn(T po,int id=MatchedID.DeleteIn, string tableIndex = "", params List<object>[] listsIn)
        {
            bool result = false;
            if (listsIn == null || listsIn.Length == 0) return result;
            try
            {
                po.DeletePropertiesByKey(id,tableIndex, listsIn.ToList());
                result = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public virtual bool UpdateByIn(T po, int id = MatchedID.UpdateIn, string tableIndex = "", List<List<object>> listsIn=null,params string[] parameters)
        {
            bool result = false;
            if (listsIn == null || listsIn.Count == 0) return result;
            try
            {
                po.UpdatePropertiesByKey(id,tableIndex, listsIn,parameters);
                result = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public virtual bool ExecuteNonQuery(T po, string sql, params object[] parameterrs)
        {
            bool result = false;
            try
            {
              result=  po.ExecuteNonQuery(sql, parameterrs);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public virtual List<TDTO> StatisticByKey<TDTO>(T po, int id = MatchedID.Statistics, string tableIndex = "", List<List<object>> listsIn = null, string sqlAppend = "")
        {
            try
            {
                return po.StatisticsByKey<TDTO>(id,tableIndex, listsIn?.ToList(),sqlAppend);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public virtual List<TDTO> SelectPropertiesBySql<TDTO>(T po, string sql, bool needParameters = false, int id = MatchedID.SelectBySql)
        {
            try
            {
                return po.SelectPropertiesBySql<TDTO>(sql, needParameters, id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
