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
    }
}
