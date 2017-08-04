using System.Text.RegularExpressions;
using System.Web;

namespace HtmlTableHelper.Injectors.Converters
{
    public class RegexColConverter : IColConverter
    {
        private readonly Regex _regex;
        private readonly string _replacement;

        public RegexColConverter(Regex regex, string replacement)
        {
            _regex = regex;
            _replacement = replacement;
        }

        public RegexColConverter(string regex, string replacement)
        {
            _regex = new Regex(regex);
            _replacement = replacement;
        }

        public string Convert(string input)
        {
            var replace = _regex.Replace(input, _replacement);
            return replace;
        }
    }
}