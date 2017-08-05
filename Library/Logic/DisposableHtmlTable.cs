using System;
using System.Collections.Generic;
using System.IO;

namespace HtmlTable.Logic
{
    /// <summary>
    /// Disposable version of the <see cref="HtmlTable{TRowModel}"/> class. An instance of this class is returned by a call to <see cref="HtmlTable{TRowModel}.Begin"/>. This class is meant to be used inside a <c>usin()</c> statement.
    /// <code>
    /// @using (DisposableHtmlTable&lt;RowViewModel&gt; table = Html.DisplayTable(m => m.ListTest).Begin)
    /// {
    ///     // Add code to configure the HTML tabel
    /// }
    /// </code>
    /// </summary>
    /// <typeparam name="TRowModel"></typeparam>
    public class DisposableHtmlTable<TRowModel> : HtmlTable<TRowModel>, IDisposable
    {
        /// <summary>
        /// Forwards all the parameters to the <see cref="HtmlTable{TRowModel}"/> class
        /// </summary>
        /// <param name="model"></param>
        /// <param name="rows"></param>
        /// <param name="writer"></param>
        public DisposableHtmlTable(object model, IEnumerable<TRowModel> rows, TextWriter writer) : base(model, rows, writer)
        {

        }

        /// <summary>
        /// Will be implictly called at the end of a <c>using(){}</c> statement. Renders the table without the need to call <see cref="HtmlTable{TRowModel}.Render()"/> explicitly
        /// </summary>
        public void Dispose()
        {
            Writer.Write(Render());
        }
    }
}