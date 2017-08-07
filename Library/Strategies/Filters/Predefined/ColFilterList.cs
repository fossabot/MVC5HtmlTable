using System.Collections.Generic;
using System.Linq;

namespace HtmlTable.Strategies.Filters.Predefined
{
    /// <summary>
    /// Returns the result of the execution of a list of <see cref="IColFilter"/>
    /// </summary>
    public class ColFilterList : IColFilter
    {
        /// <summary>
        /// Operator <see cref="ComparisonType.Or"/> or <see cref="ComparisonType.And"/>
        /// </summary>
        private readonly ComparisonType _op;
        /// <summary>
        /// The list of filters to exectue
        /// </summary>
        private readonly List<IColFilter> _filters;

        /// <summary>
        /// Instanciates a new <see cref="ColFilterList"/>
        /// </summary>
        /// <param name="filters">A <see cref="List{IColFilter}"/> of filters to exectute</param>
        /// <param name="op"><see cref="IColFilter"/></param>
        public ColFilterList(List<IColFilter> filters, ComparisonType op = ComparisonType.Or)
        {
            _filters = filters;
            _op = op;
        }

        /// <summary>
        /// Instanciates a new <see cref="ColFilterList"/>
        /// </summary>
        /// <param name="op"><see cref="IColFilter"/></param>
        /// <param name="filters">An array of <see cref="IColFilter"/> filters to execute. Passed as an array or multiple arguments</param>
        public ColFilterList(ComparisonType op, params IColFilter[] filters)
        {
            _op = op;
            _filters = filters.ToList();
        }

        /// <summary>
        /// Instanciates a new <see cref="ColFilterList"/>
        /// </summary>
        /// <param name="op"><see cref="IColFilter"/></param>
        /// <param name="filters">A <see cref="List{IColFilter}"/> of filters to exectute</param>
        public ColFilterList(IColFilter[] filters, ComparisonType op)
        {
            _op = op;
            _filters = filters.ToList();
        }

        /// <summary>
        /// Applies every <see cref="IColFilter"/> and combines them using the provided <see cref="ComparisonType"/>
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
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

    /// <summary>
    /// Allows to specify the operator used to combine the result of all the contained <see cref="IColFilter"/> contained in a <see cref="ColFilterList"/>
    /// </summary>
    public enum ComparisonType
    {
        Or,
        And
    }
}