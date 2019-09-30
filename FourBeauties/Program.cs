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
            WorkLineBO workLineBO = new WorkLineBO { EnterpriseCode = "FourBeauties", FacotryCode = "YangYuHuan", WorkLineCode = "XiShi", WorkLineName = "DiaoChan", WorkShopCode = "WangZhaoJun",WorkLineUUID=Guid.NewGuid().ToString("N") };
            workLineService.Insert(workLineBO);
            WorkLineBO workLineBOUpdate = new WorkLineBO { FacotryCode = "JoerYang", WorkLineUUID = workLineBO.WorkLineUUID };
            workLineService.UpdateByKey(workLineBOUpdate,MatchedID.Update, "factory_code");
            WorkLineBO worklineBOSearch = new WorkLineBO { FacotryCode = "JoerYang" };
            var beauties = workLineService.SelectAllByKey<WorkLineDTO,WorkLineVO>(worklineBOSearch);
            WorkLineBO workLineBOStatistics = new WorkLineBO();
            var statistics = workLineService.StatisticByKey<WorkLinePO, WorkLinePO>(workLineBOStatistics);
            workLineService.DeleteByKey(new WorkLineBO { WorkLineUUID = workLineBO.WorkLineUUID });
            Console.ReadKey();
        }
    }
}
