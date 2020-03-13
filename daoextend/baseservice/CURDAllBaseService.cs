using daoextend.basedao;
using daoextend.consts;
using daoextend.daoextend;
using daoextend.entity;
using daoextend.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace daoextend.baseservice
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class CURDAllBaseService<TBO,TPO> where TPO :ICURDAll
    {
       public virtual CURDAllBaseDao<TPO> CURDAllBaseDao { get; set; } = new CURDAllBaseDao<TPO>();
        public virtual void Insert(TBO bo,int id=MatchedID.Insert, string tableIndex = null)
        {
            try
            {
                var po = bo.ExplicitToType<TPO>();
                CURDAllBaseDao.Insert(po,id,tableIndex);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public virtual void InsertOrExistNot(TBO bo, int id = MatchedID.InsertOrUpdate, string tableIndex = null)
        {
            try
            {
                var po = bo.ExplicitToType<TPO>();
                CURDAllBaseDao.InsertOrExistNot(po, id, tableIndex);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public virtual void InsertOrUpdate(TBO bo, int id = MatchedID.InsertOrUpdate, string tableIndex = null)
        {
            try
            {
                var po = bo.ExplicitToType<TPO>();
                CURDAllBaseDao.InsertOrUpdate(po, id, tableIndex);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public virtual void InsertOrMerge(TBO bo, int id = MatchedID.InsertOrMerge, Func<TPO, TPO, TPO> func = null,bool insertMore=false, string tableIndex = null)
        {
            try
            {
                var po = bo.ExplicitToType<TPO>();
                CURDAllBaseDao.InsertOrMerge(po, id, func, insertMore, tableIndex);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public virtual void DeleteByKey(TBO bo,int id=MatchedID.Delete, string tableIndex = null)
        {
            try
            {
                var po = bo.ExplicitToType<TPO>();
                CURDAllBaseDao.DeleteByKey(po,id,tableIndex);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public virtual void UpdateByKey(TBO bo,int id=MatchedID.Update, string tableIndex = null, params string[] parameters)
        {
            try
            {
                var po = bo.ExplicitToType<TPO>();
                CURDAllBaseDao.UpdateByKey(po,id,tableIndex,parameters);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public virtual List<TVO> SelectAllByKey<TDTO,TVO>(TBO bo,int id=MatchedID.SelectAll, string tableIndex = null)
        {
            try
            {
                var po = bo.ExplicitToType<TPO>();
                var dtos = CURDAllBaseDao.SelectAllByKey<TDTO>(po,id,tableIndex);
                var vos = dtos?.Select(w => w.ExplicitToType<TVO>()).ToList();
                return vos;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public virtual PageList<TVO> SelecttAllPageListByKey<TDTO, TVO>(TBO bo, int pageIndex, int pageSize, int id = MatchedID.SelectAll, string tableIndex = null)
        {
            try
            {
                var po = bo.ExplicitToType<TPO>();
                var dtos = CURDAllBaseDao.SelectAllPageListByKey<TDTO>(po, pageIndex, pageSize, id, tableIndex);
                var vos = new PageList<TVO> { PageIndex=dtos.PageIndex,PageSize=dtos.PageSize,TotalCount=dtos.TotalCount,TotalPage=dtos.TotalPage };
                vos.ListItems= dtos?.ListItems?.Select(w => w.ExplicitToType<TVO>()).ToList();
                return vos;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public virtual TVO SelectExists<TDTO, TVO>(TBO bo, int id = MatchedID.SelectExists, string tableIndex = null)
        {
            try
            {
                var po = bo.ExplicitToType<TPO>();
                var dto = CURDAllBaseDao.SelectExists<TDTO>(po, id,tableIndex);
                var vo = dto.ExplicitToType<TVO>();
                return vo;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public virtual TVO SelectSingleByKey<TDTO, TVO>(TBO bo, int id = MatchedID.SelectSingle, string tableIndex = null)
        {
            try
            {
                var po = bo.ExplicitToType<TPO>();
                var dto = CURDAllBaseDao.SelectSingleByKey<TDTO>(po, id,tableIndex);
                var vo = dto.ExplicitToType<TVO>();
                return vo;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public virtual List<TVO> SelectByIn<TDTO, TVO>(TBO bo,int id=MatchedID.SelectIn, string tableIndex = null, params List<object>[] listsIn)
        {
            try
            {
                var po = bo.ExplicitToType<TPO>();
                var dtos = CURDAllBaseDao.SelectByIn<TDTO>(po,id,tableIndex,listsIn);
                var vos = dtos?.Select(w => w.ExplicitToType<TVO>()).ToList();
                return vos;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public virtual PageList<TVO> SelectPageListByIn<TDTO, TVO>(TBO bo, int pageIndex, int pageSize, int id = MatchedID.SelectIn, string tableIndex = null, params List<object>[] listsIn)
        {
            try
            {
                var po = bo.ExplicitToType<TPO>();
                var dtos = CURDAllBaseDao.SelectPageListByIn<TDTO>(po,pageIndex,pageSize, id, tableIndex, listsIn);
                var vos = new PageList<TVO> { PageIndex = dtos.PageIndex, PageSize = dtos.PageSize, TotalCount = dtos.TotalCount, TotalPage = dtos.TotalPage };
                vos.ListItems = dtos?.ListItems ?.Select(w => w.ExplicitToType<TVO>()).ToList();
                return vos;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public virtual bool DeleteByIn(TBO bo, int id = MatchedID.DeleteIn, string tableIndex = null, params List<object>[] listsIn)
        {
            try
            {
                var po = bo.ExplicitToType<TPO>();
                return CURDAllBaseDao.DeleteByIn(po, id,tableIndex, listsIn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }        

        public virtual bool UpdateByIn(TBO bo, int id = MatchedID.UpdateIn, string tableIndex = null, List<List<object>> listsIn=null,params string[] properties)
        {
            try
            {
                var po = bo.ExplicitToType<TPO>();
                return CURDAllBaseDao.UpdateByIn(po, id,tableIndex, listsIn,properties);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public virtual bool ExecuteNonQuery(TBO bo,  string sql, params object[] parameterrs)
        {
            bool result = false;
            try
            {
                var po = bo.ExplicitToType<TPO>();
                result = CURDAllBaseDao.ExecuteNonQuery(po,sql, parameterrs);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public virtual List<TVO> StatisticByKey<TDTO, TVO>(TBO bo, int id = MatchedID.Statistics, string tableIndex = null, List<List<object>> listsIn = null, string sqlAppend = "")
        {
            try
            {
                var po = bo.ExplicitToType<TPO>();
                var dtos = CURDAllBaseDao.StatisticByKey<TDTO>(po, id,tableIndex, listsIn);
                var vos = dtos?.Select(w => w.ExplicitToType<TVO>()).ToList();
                return vos;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public virtual List<TVO> SelectPropertiesBySql<TDTO, TVO>(TBO bo, string sql, bool needParameters = false, int id = MatchedID.SelectBySql)
        {
            try
            {
                var po = bo.ExplicitToType<TPO>();
                var dtos = CURDAllBaseDao.SelectPropertiesBySql<TDTO>(po, sql, needParameters, id);
                var vos = dtos?.Select(w => w.ExplicitToType<TVO>()).ToList();
                return vos;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
