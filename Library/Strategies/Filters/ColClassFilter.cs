namespace HtmlTable.Strategies.Filters
{
    /// <summary>
    /// Applies a list of CSS class when the given filters returns true
    /// </summary>
    public class ColClassFilter
    {
        /// <summary>
        /// The <see cref="IColFilter"/> used to decide when to apply the classes
        /// </summary>
        private readonly IColFilter _filter;
        /// <summary>
        /// A space separated list of CSS classes
        /// </summary>
        private readonly string _classes;

        /// <summary>
        /// Creates a new <see cref="ColClassFilter"/>
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="classes"></param>
        public ColClassFilter(IColFilter filter, string classes)
        {
            _filter = filter;
            _classes = classes;
        }

        /// <summary>
        /// Contains the custom logic that decides when to apply the CSS classes
        /// </summary>
        /// <param name="value"></param>
        /// <returns>A string separated list of CSS classes or an empty string</returns>
        public string GetClassIfFilterMatches(string value)
        {
            return _filter.FilterForValue(value) ? _classes : "";
        }
    }
}