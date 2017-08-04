using HtmlTableHelper.Models;

namespace HtmlTableHelper.Strategies.Injectors
{
    public interface IColDataInjector
    {
        string GetData(DataInjectorModel input);
    }
}