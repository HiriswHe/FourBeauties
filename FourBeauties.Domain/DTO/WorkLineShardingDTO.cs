using daoextend.attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace FourBeauties.Domain.DTO
{
    public class WorkLineShardingDTO
    {
        /// <summary>
        /// 
        /// </summary>
        public string workline_uuid { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [ExplicitAliasName(Name = "FacotryCode")]
        public string factory_code { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [ExplicitAliasName(Name = "WorkShopCode")]
        public string workshop_code { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [ExplicitAliasName(Name = "WorkLineCode")]
        public string workline_code { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [ExplicitAliasName(Name = "WorkLineName")]
        public string workline_name { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string enterprise_code { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime create_time { get; set; } = DateTime.Now;
        /// <summary>
        /// 
        /// </summary>
        public DateTime update_time { get; set; } = DateTime.Now;
    }
}
