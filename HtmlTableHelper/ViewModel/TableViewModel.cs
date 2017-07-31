using System.Collections.Generic;

namespace HtmlTableHelper.ViewModel
{
    public class TableViewModel
    {
        public List<string> Header { get; set; }
        public List<List<string>> Rows { get; set; } 
    }
}