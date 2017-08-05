using System;
using System.Collections.Generic;
using System.IO;

namespace HtmlTable.Logic
{
    public class DisposableHtmlTable<TRowModel> : HtmlTable<TRowModel>, IDisposable
    {
        public DisposableHtmlTable(object model, IEnumerable<TRowModel> rows, TextWriter writer) : base(model, rows, writer)
        {

        }

        public void Dispose()
        {
            Writer.Write(Render());
        }
    }
}