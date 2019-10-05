using daoextend.utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace daoextend.config
{
    public class CommonHelper
    {
        public static bool LogSql = false;
        static CommonHelper()
        {
            string sLogSql = AppSetting.GetConfig("daoextend:logsql");
            if (string.Compare(sLogSql, "true", true) == 0)
            {
                LogSql = true;
            }
            else
                LogSql = false;
        }
    }
}
