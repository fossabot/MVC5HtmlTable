using HtmlTable.Models;

namespace HtmlTable.Strategies.Injectors
{
    public interface IColDataInjector
    {
        string GetData(DataInjectorModel input);
    }
}