using System;
using System.Collections.Generic;
using System.Text;

namespace daoextend.daoextend
{
    public static class ListExtend
    {
        public static List<List<T>> SplitByMaxCount<T>(this List<T> list, int maxCount = 1000)
        {
            List<List<T>> result = new List<List<T>>();
            if (list == null || list.Count == 0) return result;
            List<T> innerList = new List<T>();
            for (int i = 0; i < list.Count; i++)
            {
                for (int j = 0; j < maxCount && i < list.Count; j++, i++)
                {
                    innerList.Add(list[i]);
                    if (j == maxCount - 1 || i == list.Count - 1)
                    {
                        result.Add(innerList);
                        innerList = new List<T>();
                        break;
                    }
                }
            }
            return result;
        }
    }
}
