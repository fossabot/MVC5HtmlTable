using System.Collections.Generic;
using System.Linq;
using HtmlTable.Models;
using HtmlTable.Strategies.Converters;
using HtmlTable.Strategies.Filters;
using HtmlTable.Strategies.Injectors;

namespace HtmlTable.ViewModel
{
    public class TableViewModel
    {
        public List<string> Header { get; set; }

        public List<string> RenamedHeader => Header.Select(h =>
                                                                GlobalRenameMapping.ContainsKey(h) ? GlobalRenameMapping[h] :
                                                               (HeaderRenameMapping.ContainsKey(h) ? HeaderRenameMapping[h] : h)).ToList();

        public List<string> RenamedFooter => Header.Select(h =>
                                                                GlobalRenameMapping.ContainsKey(h) ? GlobalRenameMapping[h] :
                                                               (FooterRenameMapping.ContainsKey(h) ? FooterRenameMapping[h] : h)).ToList();

        public List<List<IColDataInjector>> Rows { get; set; }
        public Dictionary<string, string> GlobalRenameMapping { get; set; } = new Dictionary<string, string>();
        public Dictionary<string, string> HeaderRenameMapping { get; set; } = new Dictionary<string, string>();
        public Dictionary<string, string> FooterRenameMapping { get; set; } = new Dictionary<string, string>();
        public Dictionary<string, IColFilter> FiltersMapping { get; set; } = new Dictionary<string, IColFilter>();
        public Dictionary<string, ColClassFilter> ColsClassMapping { get; set; } = new Dictionary<string, ColClassFilter>();
        public Dictionary<string, IColConverter> ConvertersMapping { get; set; } = new Dictionary<string, IColConverter>();
        public TableOptions TableOptions { get; set; } = new TableOptions();
        public string RootClasses { get; set; }
    }
}