using System.Collections.Generic;
using System.Linq;

namespace HtmlTable.Strategies.Converters.Predefined
{
    /// <summary>
    /// Contains a list of <see cref="IColConverter"/> to apply on a column.
    /// </summary>
    public class ColConverterList : IColConverter
    {
        /// <summary>
        /// The list of <see cref="IColConverter"/>
        /// </summary>
        private readonly List<IColConverter> _filters;

        /// <summary>
        /// Instanciates a new <see cref="ColConverterList"/> from a <see cref="List{IColConverter}"/>
        /// </summary>
        /// <param name="filters"></param>
        public ColConverterList(List<IColConverter> filters)
        {
            _filters = filters;
        }

        /// <summary>
        /// Instanciates a new <see cref="ColConverterList"/> from either an array of <see cref="IColConverter"/> or mutiple <see cref="IColConverter"/> passed as arguments
        /// </summary>
        /// <param name="filters"></param>
        public ColConverterList(params IColConverter[] filters)
        {
            _filters = filters.ToList();
        }

        /// <summary>
        /// Executes all the <see cref="IColConverter"/>s
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public string Convert(string input)
        {
            return _filters.Aggregate(input, (current, filter) => filter.Convert(current));
        }
    }

}