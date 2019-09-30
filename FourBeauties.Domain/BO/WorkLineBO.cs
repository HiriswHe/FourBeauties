using daoextend.attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace FourBeauties.Domain.BO
{
    public class WorkLineBO
    {
        /// <summary>
        /// 
        /// </summary>
        [ExplicitAliasName(Name = "workline_uuid")]
        public string WorkLineUUID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [ExplicitAliasName(Name = "factory_code")]
        public string FacotryCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [ExplicitAliasName(Name = "workshop_code")]
        public string WorkShopCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [ExplicitAliasName(Name = "workline_code")]
        public string WorkLineCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [ExplicitAliasName(Name = "workline_name")]
        public string WorkLineName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [ExplicitAliasName(Name = "enterprise_code")]
        public string EnterpriseCode { get; set; }
    }
}
