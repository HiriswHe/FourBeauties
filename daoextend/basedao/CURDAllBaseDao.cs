using daoextend.consts;
using daoextend.daoextend;
using daoextend.entity;
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

        public virtual void Sharding(T po,  string tableIndex = null)
        {
            var itableSharding = po as ITableSharding;
            itableSharding.TableSharding(MatchedID.All, tableIndex);
            var idatabaseSharding = po as IDataBaseSharding;
            idatabaseSharding.DataBaseSharding(MatchedID.All, tableIndex);
        }

        public virtual void Insert(T po,int id=MatchedID.Insert, string tableIndex = null)
        {
            try
            {
                if (string.IsNullOrEmpty(po?.UUID)) po.UUID = Guid.NewGuid().ToString("N");
                Sharding(po, tableIndex);
                po.InsertProperties(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public virtual void InsertOrExistNot(T po, int id = MatchedID.InsertOrUpdate, string tableIndex = null)
        {
            try
            {
                InsertOrMerge(po, id, null, false,tableIndex);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public virtual void InsertOrUpdate(T po,int id=MatchedID.InsertOrUpdate, string tableIndex =null)
        {
            try
            {
                InsertOrMerge(po, id, (_old, _new) => { return _new; },false,tableIndex);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public virtual void InsertOrMerge(T po, int id = MatchedID.InsertOrMerge, Func<T, T, T> func=null,bool insertMore=false, string tableIndex = null)
        {
            try
            {
                if (string.IsNullOrEmpty(po?.UUID)) po.UUID = Guid.NewGuid().ToString("N");
                Sharding(po, tableIndex);
                var dtoExist = SelectSingleByKey<T>(po, id, tableIndex);
                if (dtoExist == null) Insert(po, id, tableIndex);
                else
                {
                    if (func == null) return;
                    var poNew = func.Invoke(dtoExist, po);
                    if (!insertMore)
                        UpdateByKey(poNew, id, tableIndex);
                    else
                        InsertOrMerge(poNew, id, func, false, tableIndex);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public virtual void DeleteByKey(T po,int id=MatchedID.Delete, string tableIndex = null)
        {
            try
            {
                Sharding(po, tableIndex);
                po.DeletePropertiesByKey(id,tableIndex);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public virtual void UpdateByKey(T po,int id=MatchedID.Update, string tableIndex = null, params string[] parameters)
        {
            try
            {
                Sharding(po, tableIndex);
                po.UpdatePropertiesByKey(id,tableIndex,null,parameters);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public virtual List<TDTO> SelectAllByKey<TDTO>(T po,int id=MatchedID.SelectAll,string tableIndex = null)
        {
            try
            {
                Sharding(po, tableIndex);
                return po.SelectPropertiesByKey<TDTO>(id,tableIndex);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public virtual PageList<TDTO> SelectAllPageListByKey<TDTO>(T po, int pageIndex, int pageSize, int id = MatchedID.SelectAll, string tableIndex = null)
        {
            try
            {
                Sharding(po, tableIndex);
                return po.SelectPageListPropertiesByKey<TDTO>(pageIndex,pageSize,id, tableIndex);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public virtual TDTO SelectExists<TDTO>(T po, int id = MatchedID.SelectExists, string tableIndex = null)
        {
            try
            {
                Sharding(po, tableIndex);
                return po.SelectPropertiesExists<TDTO>(id,tableIndex).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public virtual TDTO SelectSingleByKey<TDTO>(T po,int id=MatchedID.SelectSingle, string tableIndex = null)
        {
            try
            {
                Sharding(po, tableIndex);
                return po.SelectPropertiesByKey<TDTO>(id,tableIndex).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        public virtual List<TDTO> SelectByIn<TDTO>(T po, int id = MatchedID.SelectIn, string tableIndex = null, params List<object>[] listsIn)
        {
            try
            {
                Sharding(po, tableIndex);
                return po.SelectPropertiesByKey<TDTO>(id,tableIndex, listsIn?.ToList());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public virtual PageList<TDTO> SelectPageListByIn<TDTO>(T po, int pageIndex, int pageSize, int id = MatchedID.SelectIn, string tableIndex = null, params List<object>[] listsIn)
        {
            try
            {
                Sharding(po, tableIndex);
                return po.SelectPageListPropertiesByKey<TDTO>(pageIndex,pageSize, id, tableIndex, listsIn?.ToList());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public virtual bool DeleteByIn(T po,int id=MatchedID.DeleteIn, string tableIndex = null, params List<object>[] listsIn)
        {
            bool result = false;
            if (listsIn == null || listsIn.Length == 0) return result;
            try
            {
                Sharding(po, tableIndex);
                po.DeletePropertiesByKey(id,tableIndex, listsIn.ToList());
                result = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public virtual bool UpdateByIn(T po, int id = MatchedID.UpdateIn, string tableIndex = null, List<List<object>> listsIn=null,params string[] parameters)
        {
            bool result = false;
            if (listsIn == null || listsIn.Count == 0) return result;
            try
            {
                Sharding(po, tableIndex);
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
                result =  po.ExecuteNonQuery(sql, parameterrs);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public virtual List<TDTO> StatisticByKey<TDTO>(T po, int id = MatchedID.Statistics, string tableIndex = null, List<List<object>> listsIn = null, string sqlAppend = "")
        {
            try
            {
                Sharding(po, tableIndex);
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
