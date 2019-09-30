using daoextend.basedao;
using daoextend.consts;
using daoextend.daoextend;
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
        public virtual void Insert(TBO bo,int id=MatchedID.Insert)
        {
            try
            {
                var po = bo.ExplicitToType<TPO>();
                CURDAllBaseDao.Insert(po,id);
            }
            catch (Exception ex)
            {
            }
        }

        public virtual void DeleteByKey(TBO bo,int id=MatchedID.Delete)
        {
            try
            {
                var po = bo.ExplicitToType<TPO>();
                CURDAllBaseDao.DeleteByKey(po,id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public virtual void UpdateByKey(TBO bo,int id=MatchedID.Update,params string[] parameters)
        {
            try
            {
                var po = bo.ExplicitToType<TPO>();
                CURDAllBaseDao.UpdateByKey(po,id,parameters);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public virtual List<TVO> SelectAllByKey<TDTO,TVO>(TBO bo,int id=MatchedID.SelectAll)
        {
            try
            {
                var po = bo.ExplicitToType<TPO>();
                var dtos = CURDAllBaseDao.SelectAllByKey<TDTO>(po,id);
                var vos = dtos?.Select(w => w.ExplicitToType<TVO>()).ToList();
                return vos;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public virtual TVO SelectExists<TDTO, TVO>(TBO bo, int id = MatchedID.SelectExists)
        {
            try
            {
                var po = bo.ExplicitToType<TPO>();
                var dto = CURDAllBaseDao.SelectExists<TDTO>(po, id);
                var vo = dto.ExplicitToType<TVO>();
                return vo;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public virtual TVO SelectSingleByKey<TDTO, TVO>(TBO bo, int id = MatchedID.SelectSingle)
        {
            try
            {
                var po = bo.ExplicitToType<TPO>();
                var dto = CURDAllBaseDao.SelectSingleByKey<TDTO>(po, id);
                var vo = dto.ExplicitToType<TVO>();
                return vo;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public virtual List<TVO> SelectByIn<TDTO, TVO>(TBO bo,int id=MatchedID.SelectIn,params List<object>[] listsIn)
        {
            try
            {
                var po = bo.ExplicitToType<TPO>();
                var dtos = CURDAllBaseDao.SelectByIn<TDTO>(po,id,listsIn);
                var vos = dtos?.Select(w => w.ExplicitToType<TVO>()).ToList();
                return vos;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public virtual bool DeleteByIn(TBO bo, int id = MatchedID.DeleteIn, params List<object>[] listsIn)
        {
            try
            {
                var po = bo.ExplicitToType<TPO>();
                return CURDAllBaseDao.DeleteByIn(po, id, listsIn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }        

        public virtual bool UpdateByIn(TBO bo, int id = MatchedID.UpdateIn, List<List<object>> listsIn=null,params string[] properties)
        {
            try
            {
                var po = bo.ExplicitToType<TPO>();
                return CURDAllBaseDao.UpdateByIn(po, id, listsIn,properties);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public virtual bool ExecuteNonQuery(TBO bo, string sql, params object[] parameterrs)
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

        public virtual List<TVO> StatisticByKey<TDTO, TVO>(TBO bo, int id = MatchedID.Statistics, List<List<object>> listsIn = null, string sqlAppend = "")
        {
            try
            {
                var po = bo.ExplicitToType<TPO>();
                var dtos = CURDAllBaseDao.StatisticByKey<TDTO>(po, id, listsIn);
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
