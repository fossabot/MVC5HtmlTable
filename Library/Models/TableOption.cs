namespace HtmlTable.Models
{
    /// <summary>
    /// Contains the options that can be given to <see cref="TableOptions"/>
    /// </summary>
    public class TableOption
    {
        /// <summary>
        /// A reference to the parts of a table on which the library can apply options
        /// </summary>
        public enum Part
        {
            Header,
            Footer
        }
    }
}