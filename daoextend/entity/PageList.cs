using System;
using System.Collections.Generic;
using System.Text;

namespace daoextend.entity
{
    public class PageList<T>
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public long TotalCount { get; set; }
        public long TotalPage { get; set; }
        public List<T> ListItems { get; set; } = new List<T>();
    }
}
