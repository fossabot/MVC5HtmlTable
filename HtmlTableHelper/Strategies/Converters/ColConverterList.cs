using System.Collections.Generic;
using System.Linq;

namespace HtmlTableHelper.Injectors.Converters
{
    public class ColConverterList : IColConverter
    {
        private readonly List<IColConverter> _filters;

        public ColConverterList(List<IColConverter> filters)
        {
            _filters = filters;
        }

        public ColConverterList(params IColConverter[] filters)
        {
            _filters = filters.ToList();
        }

        public string Convert(string input)
        {
            return _filters.Aggregate(input, (current, filter) => filter.Convert(current));
        }
    }

}