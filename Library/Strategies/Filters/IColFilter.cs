namespace HtmlTable.Strategies.Filters
{
    /// <summary>
    /// Allows to implement custom logic to decide whether a column will be showed for each row
    /// </summary>
    public interface IColFilter
    {
        /// <summary>
        /// Contains the custom filtering logic.
        /// </summary>
        /// <param name="value">The raw column data for the current row</param>
        /// <returns>True if the column should be shown, false if it should be hidden</returns>
        bool FilterForValue(string value);
    }
}