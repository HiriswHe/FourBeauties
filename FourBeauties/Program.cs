using daoextend.consts;
using daoextend.sharding;
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
        static WorkLineShardingService workLineShardingService = new WorkLineShardingService();
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            WorkLineShardingBO workLineShardingBOSearchOracle = new WorkLineShardingBO { FacotryCode = "JoerYang", WorkLineCode = "1", WorkLineName = "DiaoChan", WorkLineUUID = "2" };
            var beautiesPageListOracle = workLineShardingService.SelecttAllPageLisByKey<WorkLineShardingDTO, WorkLineShardingVO>(workLineShardingBOSearchOracle, 2, 3);

            #region AutoSharding
            WorkLineShardingBO workLineShardingBO = new WorkLineShardingBO
            { EnterpriseCode = "FourBeauties", FacotryCode = "YangYuHuan", WorkLineCode = "XiShi", WorkLineName = "DiaoChan", WorkShopCode = "WangZhaoJun",
                WorkLineUUID = Guid.NewGuid().ToString("N") };
            workLineShardingService.Insert(workLineShardingBO);
            WorkLineShardingBO workLineShardingBOUpdate = new WorkLineShardingBO { FacotryCode = "JoerYang", WorkLineUUID = workLineShardingBO.WorkLineUUID };
            workLineShardingService.UpdateByKey(workLineShardingBOUpdate, MatchedID.Update, null, "factory_code","workline_code='1'");
            WorkLineShardingBO workLineShardingBOSearch = new WorkLineShardingBO { FacotryCode = "JoerYang",WorkLineCode= "1",WorkLineName= "DiaoChan", WorkLineUUID = workLineShardingBO.WorkLineUUID };
            var beauties0 = workLineShardingService.SelectAllByKey<WorkLineShardingDTO,WorkLineShardingVO>(workLineShardingBOSearch);

            var beautiesPageList = workLineShardingService.SelecttAllPageLisByKey<WorkLineShardingDTO, WorkLineShardingVO>(workLineShardingBOSearch, 2, 3);

            WorkLineShardingBO workLineShardingBOStatistic = new WorkLineShardingBO { WorkLineUUID = workLineShardingBO.WorkLineUUID };
            var statistics0= workLineShardingService.StatisticByKey<WorkLineShardingPO, WorkLineShardingPO>(workLineShardingBOStatistic);
            workLineShardingService.DeleteByKey(new WorkLineShardingBO { WorkLineUUID = workLineShardingBO.WorkLineUUID });
            #endregion

            #region Manual Sharding
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
            #endregion

            Console.ReadKey();
        }
    }
}
