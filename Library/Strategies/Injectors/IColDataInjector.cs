using HtmlTable.Logic;
using HtmlTable.Models;

namespace HtmlTable.Strategies.Injectors
{
    /// <summary>
    /// Generates the data for virtual columns added via the <see cref="HtmlTable{TRowModel}.AddColumn(string,HtmlTable.Strategies.Injectors.IColDataInjector)"/> method
    /// </summary>
    public interface IColDataInjector
    {
        /// <summary>
        /// Returns the generated value
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        string GetData(DataInjectorModel input);
    }
}