using System.Collections.Generic;
using System.Linq;
using HtmlTable.Models;
using HtmlTable.Strategies.Converters;
using HtmlTable.Strategies.Filters;
using HtmlTable.Strategies.Injectors;

namespace HtmlTable.ViewModel
{
    /// <summary>
    /// Contains all the information used by the Razor view to generate the HTML table
    /// </summary>
    public class TableViewModel
    {
        /// <summary>
        /// Contains the list of columns to be displayed. These values are more of "identifiers", that will be used to display in the table header if no name override is found (see <see cref="RenamedHeader"/> and <see cref="RenamedFooter"/>)
        /// </summary>
        public List<string> ColumnsName { get; set; }

        /// <summary>
        /// Returns the list of column names after applying any renames, globals or specific to the header (see <see cref="GlobalRenameMapping"/> and <see cref="HeaderRenameMapping"/>)
        /// </summary>
        public List<string> RenamedHeader => ColumnsName.Select(h =>
                                                                GlobalRenameMapping.ContainsKey(h) ? GlobalRenameMapping[h] :
                                                               (HeaderRenameMapping.ContainsKey(h) ? HeaderRenameMapping[h] : h)).ToList();

        /// <summary>
        /// Returns the list of column names after applying any renames, globals or specific to the footer (see <see cref="GlobalRenameMapping"/> and <see cref="FooterRenameMapping"/>)
        /// </summary>
        public List<string> RenamedFooter => ColumnsName.Select(h =>
                                                                GlobalRenameMapping.ContainsKey(h) ? GlobalRenameMapping[h] :
                                                               (FooterRenameMapping.ContainsKey(h) ? FooterRenameMapping[h] : h)).ToList();

        /// <summary>
        /// Global rename mapping. Will be applied on both the header and footer. This mapping will be hidden by more specific mappings in case of collision (see <see cref="HeaderRenameMapping"/> and <see cref="FooterRenameMapping"/>)
        /// </summary>
        public Dictionary<string, string> GlobalRenameMapping { get; set; } = new Dictionary<string, string>();

        /// <summary>
        /// Rename mapping for the header for the header of the HTML table. Will override <see cref="GlobalRenameMapping"/>
        /// </summary>
        public Dictionary<string, string> HeaderRenameMapping { get; set; } = new Dictionary<string, string>();

        /// <summary>
        /// Rename mapping for the header for the footer of the HTML table. Will override <see cref="GlobalRenameMapping"/>
        /// </summary>
        public Dictionary<string, string> FooterRenameMapping { get; set; } = new Dictionary<string, string>();

        /// <summary>
        /// Maps a column's identifier (see <see cref="ColumnsName"/>) to a <see cref="IColFilter"/> to display or hide the given column for each row
        /// </summary>
        public Dictionary<string, IColFilter> FiltersMapping { get; set; } = new Dictionary<string, IColFilter>();

        /// <summary>
        /// Maps a column's identifier (see <see cref="ColumnsName"/>) to a <see cref="ColClassFilter"/> to apply a CSS class to a given column for each row
        /// </summary>
        public Dictionary<string, ColClassFilter> ColsClassMapping { get; set; } = new Dictionary<string, ColClassFilter>();

        /// <summary>
        /// Maps a column's identifier (see <see cref="ColumnsName"/>) to a <see cref="IColConverter"/> to modify the data in a given column for each row
        /// </summary>
        public Dictionary<string, IColConverter> ConvertersMapping { get; set; } = new Dictionary<string, IColConverter>();

        /// <summary>
        /// Contains the information relative to the parts of the table to be displayed and their specific options
        /// </summary>
        public TableOptions TableOptions { get; set; } = new TableOptions();

        /// <summary>
        /// The list of CSS classes to apply on the &lt;table&gt; DOM element
        /// </summary>
        public string RootClasses { get; set; }

        /// <summary>
        /// A list of rows each containing a list of <see cref="IColDataInjector"/> that represent the data for each column (see <see cref="ColumnsName"/>)
        /// </summary>
        public List<List<IColDataInjector>> Rows { get; set; }
    }
}