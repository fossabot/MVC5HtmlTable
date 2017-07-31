using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
