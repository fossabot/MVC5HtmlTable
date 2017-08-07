using System.Collections.Generic;

namespace HtmlTable.Models
{
    public class TableOptions
    {
        public Dictionary<Table.Part, bool> PartsStatus { get; set; } = new Dictionary<Table.Part, bool>
        {
            {Table.Part.Header, true },
            {Table.Part.Footer, false }
        };
        public bool IsHeaderEnabled => PartsStatus.ContainsKey(Table.Part.Header) && PartsStatus[Table.Part.Header];
        public bool IsFooterEnabled => PartsStatus.ContainsKey(Table.Part.Footer) && PartsStatus[Table.Part.Footer];
    }
}