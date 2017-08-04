using System.Collections.Generic;
using System.Linq;
using HtmlTableHelper.Injectors.Filters;

namespace HtmlTableHelper.Strategies.Filters.Predefined
{
    public class ColFilterList : IColFilter
    {
        private readonly ComparisonType _op;
        private readonly List<IColFilter> _filters;

        public ColFilterList(List<IColFilter> filters, ComparisonType op = ComparisonType.Or)
        {
            _filters = filters;
            _op = op;
        }

        public ColFilterList(ComparisonType op, params IColFilter[] filters)
        {
            _op = op;
            _filters = filters.ToList();
        }

        public ColFilterList(IColFilter[] filters, ComparisonType op)
        {
            _op = op;
            _filters = filters.ToList();
        }

        public bool FilterForValue(string value)
        {
            var rtrn = false;
            
            if(_op == ComparisonType.Or)
                rtrn = _filters.Select(f => f.FilterForValue(value)).Any(f => f);

            if (_op == ComparisonType.And)
                rtrn = _filters.Select(f => f.FilterForValue(value)).All(f => f);

            return rtrn;
        }
    }
    public enum ComparisonType
    {
        Or,
        And
    }
}