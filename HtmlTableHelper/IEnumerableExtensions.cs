using System;
using System.Collections;

namespace HtmlTableHelper
{
    public static class ListExtensions
    {
        public static Type GetContainedType(this IList l)
        {
            if (l.Count == 0)
                throw new ArgumentException("List can not be empty");

            return l[0].GetType();
        }
    }
}
