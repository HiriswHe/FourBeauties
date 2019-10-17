using daoextend.attributes;
using daoextend.enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace daoextend.consts
{
    public class MatchedID
    {
        public const int Insert = 0;
        public const int Delete = 1;
        public const int Update = 2;
        public const int SelectAll = 3;
        public const int SelectSingle = 4;
        public const int SelectIn = 5;
        public const int DeleteIn = 6;
        public const int UpdateIn = 7;
        public const int SelectExists = 8;
        public const int InsertBySql = 10;
        public const int DeleteBySql = 11;
        public const int UpdateBySql = 12;
        public const int SelectBySql = 13;
        public const int Statistics = 88;
        public const int All = 99;
    }

    public class Command
    {
        public const string Max = "Max";
        public const string Min = "Min";
        public const string Sum = "Sum";
        public const string Avg = "Avg";
        public const string Count = "Count";
        public const string GroupBy = "Group By";
        public const string OrderBy = "Order By";
        public const string Asc = "Asc";
        public const string Desc = "Desc";
    }

    public class PageListSql
    {
        public const string PageListOracle = @"SELECT *
  FROM(SELECT tt.*, ROWNUM AS rowno
          FROM ({0}) tt
         WHERE ROWNUM <= {1}) table_alias
 WHERE table_alias.rowno > {2};";//{0} Select Sql, {1} Finshed Index ,{2} Started Index 
        public const string PageListSqlServer = @"select top {0} * 
from ({1} ) temp_row
where rownumber>(({2}-1)*{0}) ";
        public const string PageListSqlServer1 = "{0} as rownumber, ";
        public const string PageListSqlServer1_0 = "row_number() over({0})";
        /*[{0}] top {0}
         [{0}][{0}] pageSize
         [{1}] {0} as rownumber 
         [{1}][{0}]  row_number() over({0})
         [{1}][{0}][{0}] order by *** asc
         select top pageSize * 
from (select row_number() 
over(order by sno asc) as rownumber,* 
from student) temp_row
where rownumber>((pageIndex-1)*pageSize);
         */
    }
}
