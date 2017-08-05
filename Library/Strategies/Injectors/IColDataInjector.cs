using Library.Models;

namespace Library.Strategies.Injectors
{
    public interface IColDataInjector
    {
        string GetData(DataInjectorModel input);
    }
}