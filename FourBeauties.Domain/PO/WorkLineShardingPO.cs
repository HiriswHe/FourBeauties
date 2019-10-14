using daoextend.attributes;
using daoextend.consts;
using daoextend.enums;
using daoextend.interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace FourBeauties.Domain.PO
{
    [MatchedTable(DBServerType = DBServerType.MySql, ConnectionKey = "ConnectionStrings:produce", Name = "workline{0}")]
    [DataBaseSharding(DataBaseShardingBegin ="1",DataBaseShardingEnd ="2", ShardingMaxCount = int.MaxValue,
        ShardingAlgorithm = "daoextend.sharding.HashDevideSharding")]
    [TableSharding(TableShardingBegin ="1",TableShardingEnd ="2",ShardingTotalCount =2,
        ShardingAlgorithm = "daoextend.sharding.HashModSharding")]
    public class WorkLineShardingPO : ICURDAll,ITableSharding,IDataBaseSharding
    {
        public string __TableIndex__ { get; set ; }
        public string __DataBaseIndex__ { get; set ; }
        [Ignore(ID = MatchedID.All)]
        public string UUID
        {
            get { return workline_uuid; }
            set { workline_uuid = value; }
        }

        [Statistics(Column = "workline_uuid", Command = Command.Count, ID = MatchedID.Statistics)]
        [Ignore(ID = MatchedID.All)]
        public int TotalUUId { get; set; }

        [Statistics(Column = "factory_code", Command = Command.Max, ID = MatchedID.Statistics)]
        [Ignore(ID =MatchedID.All)]
        public string MaxFactoryCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [MatchedColumn(ID = MatchedID.Delete)]
        [MatchedColumn(ID = MatchedID.Update)]
        [ShardingColumn(Key ="HashKey")]
        public string workline_uuid { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Statistics(Command = Command.OrderBy, SType = StatisticsType.Footer,Column = "factory_code")]
        [MatchedColumn(ID = MatchedID.SelectAll,IgnoreHashCode = "null")]
        public string factory_code { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Statistics(Command = Command.GroupBy, SType = StatisticsType.Footer,Column = "workshop_code")]
        [MatchedColumn(ID = MatchedID.SelectAll,IgnoreHashCode = "null")]
        public string workshop_code { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [MatchedColumn(ID = MatchedID.SelectAll, IgnoreHashCode = "null")]
        public string workline_code { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [MatchedColumn(ID = MatchedID.SelectAll, IgnoreHashCode = "null",ContactNotation ="like")]
        public string workline_name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string enterprise_code { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Ignore(ID = MatchedID.Update)]
        public DateTime create_time { get; set; } = DateTime.Now;

        /// <summary>
        /// 
        /// </summary>
        public DateTime update_time { get; set; } = DateTime.Now;
    }
}
