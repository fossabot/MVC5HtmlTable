using System.Text.RegularExpressions;

namespace HtmlTable.Strategies.Converters.Predefined
{
    /// <summary>
    /// Use a <see cref="Regex"/> to replace some text by the give string in the input string
    /// </summary>
    public class RegexColConverter : IColConverter
    {
        /// <summary>
        /// The Regex use to find the strings to replace
        /// </summary>
        private readonly Regex _regex;
        /// <summary>
        /// The new value to put instead of the original string(s)
        /// </summary>
        private readonly string _replacement;

        /// <summary>
        /// Instanciates a <see cref="RegexColConverter"/>
        /// </summary>
        /// <param name="regex"></param>
        /// <param name="replacement"></param>
        public RegexColConverter(Regex regex, string replacement)
        {
            _regex = regex;
            _replacement = replacement;
        }

        /// <summary>
        /// Instanciates a <see cref="RegexColConverter"/>
        /// </summary>
        /// <param name="regex">String - will be used to instanciate a new <see cref="Regex"/></param>
        /// <param name="replacement"></param>
        public RegexColConverter(string regex, string replacement)
        {
            _regex = new Regex(regex);
            _replacement = replacement;
        }

        /// <summary>
        /// Method called while rendering the table. This is where the transformation takes palce.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public string Convert(string input)
        {
            var replace = _regex.Replace(input, _replacement);
            return replace;
        }
    }
}