using daoextend.consts;
using FourBeauties.Domain.BO;
using FourBeauties.Domain.DTO;
using FourBeauties.Domain.PO;
using FourBeauties.Domain.VO;
using FourBeauties.Service;
using System;
using System.Linq;

namespace FourBeauties
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            WorkLineService workLineService = new WorkLineService();
            WorkLineBO workLineBO = new WorkLineBO {
                __DataBaseIndex__="1",__TableIndex__="2",//Support DataBase Sharding And Table Sharding
                EnterpriseCode = "FourBeauties", FacotryCode = "YangYuHuan", WorkLineCode = "XiShi", WorkLineName = "DiaoChan", WorkShopCode = "WangZhaoJun",WorkLineUUID=Guid.NewGuid().ToString("N") };
            workLineService.Insert(workLineBO);
            WorkLineBO workLineBOUpdate = new WorkLineBO {
                __DataBaseIndex__ = "1",__TableIndex__ = "2",//Support DataBase Sharding And Table Sharding
                FacotryCode = "JoerYang", WorkLineUUID = workLineBO.WorkLineUUID };
            workLineService.UpdateByKey(workLineBOUpdate,MatchedID.Update,null, "factory_code");
            WorkLineBO worklineBOSearch = new WorkLineBO {
                __DataBaseIndex__ = "1",__TableIndex__ = "2",//Support DataBase Sharding And Table Sharding
                FacotryCode = "JoerYang" };
            var beauties = workLineService.SelectAllByKey<WorkLineDTO,WorkLineVO>(worklineBOSearch);
            WorkLineBO workLineBOStatistics = new WorkLineBO
            {
                __DataBaseIndex__ = "1",__TableIndex__ = "2",//Support DataBase Sharding And Table Sharding
            };
            var statistics = workLineService.StatisticByKey<WorkLinePO, WorkLinePO>(
                workLineBOStatistics);
            workLineService.DeleteByKey(new WorkLineBO {
                __DataBaseIndex__ = "1",__TableIndex__ = "2",//Support DataBase Sharding And Table Sharding
                WorkLineUUID = workLineBO.WorkLineUUID });
                Console.ReadKey();
        }
    }
}
