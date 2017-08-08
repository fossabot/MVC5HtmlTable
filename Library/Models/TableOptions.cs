using System.Collections.Generic;
using HtmlTable.Logic;

namespace HtmlTable.Models
{
    /// <summary>
    /// The list of currently set options
    /// </summary>
    public class TableOptions
    {
        /// <summary>
        /// Maps a <see cref="TableOption.Part"/> to a boolean telling if the given part should be shown or hidden in the rendered table
        /// </summary>
        public Dictionary<TableOption.Part, bool> PartsDispalyMapping { get; set; } = new Dictionary<TableOption.Part, bool>
        {
            {TableOption.Part.Header, true },
            {TableOption.Part.Footer, false }
        };

        /// <summary>
        /// Uses the <see cref="PartsDispalyMapping"/> proprety to know wether the generate HTML table should have a header section
        ///  </summary>
        public bool IsHeaderEnabled => PartsDispalyMapping.ContainsKey(TableOption.Part.Header) && PartsDispalyMapping[TableOption.Part.Header];

        /// <summary>
        /// Uses the <see cref="PartsDispalyMapping"/> proprety to know wether the generate HTML table should have a footer section
        ///  </summary>
        public bool IsFooterEnabled => PartsDispalyMapping.ContainsKey(TableOption.Part.Footer) && PartsDispalyMapping[TableOption.Part.Footer];
    }
}