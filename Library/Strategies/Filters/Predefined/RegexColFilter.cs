using System.Text.RegularExpressions;

namespace HtmlTable.Strategies.Filters.Predefined
{
    /// <summary>
    /// Decides whether to display a column in a row using a regex match
    /// </summary>
    public class RegexColFilter : IColFilter
    {
        /// <summary>
        /// The regex used for the match
        /// </summary>
        private readonly Regex _regex;

        /// <summary>
        /// Instanciates a new <see cref="RegexColFilter"/>
        /// </summary>
        /// <param name="regex">Will be converted to a <see cref="Regex"/></param>
        public RegexColFilter(string regex)
        {
            _regex = new Regex(regex);
        }

        /// <summary>
        /// Instanciates a new <see cref="RegexColFilter"/>
        /// </summary>
        /// <param name="regex"></param>
        public RegexColFilter(Regex regex)
        {
            _regex = regex;
        }

        /// <summary>
        /// Uses the provided <see cref="Regex"/> to decided whether to display or hide the column in the current row
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool FilterForValue(string value)
        {
            return _regex.IsMatch(value);
        }
    }
}