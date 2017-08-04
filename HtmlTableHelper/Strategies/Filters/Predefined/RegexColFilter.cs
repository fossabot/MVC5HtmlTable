using System.Text.RegularExpressions;
using HtmlTableHelper.Injectors.Filters;

namespace HtmlTableHelper.Strategies.Filters.Predefined
{
    public class RegexColFilter : IColFilter
    {
        private readonly Regex _regex;

        public RegexColFilter(string regex)
        {
            _regex = new Regex(regex);
        }
        public RegexColFilter(Regex regex)
        {
            _regex = regex;
        }

        public bool FilterForValue(string value)
        {
            return _regex.IsMatch(value);
        }
    }
}