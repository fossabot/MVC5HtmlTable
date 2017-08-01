using System.Collections.Generic;
using System.Linq;

namespace HtmlTableHelper.ViewModel
{
    public class TableViewModel
    {public List<string> Header { get; set; }

        public List<string> RenamedHeader => Header.Select(h => HeaderRenameMapping.ContainsKey(h) ? HeaderRenameMapping[h] : h).ToList();

        public List<List<string>> Rows { get; set; }
        public Dictionary<string, string> HeaderRenameMapping { get; set; }
        public TableOptions TableOptions { get; set; } = new TableOptions();
    }
}