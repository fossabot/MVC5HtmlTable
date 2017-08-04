using HtmlTableHelper.Models;

namespace HtmlTableHelper.Strategies.Injectors
{
    public class StringColDataInjector : IColDataInjector
    {
        private readonly string _value;

        public StringColDataInjector(string value)
        {
            _value = value;
        }

        public string GetData(DataInjectorModel input)
        {
            return _value;
        }

        static public implicit operator StringColDataInjector(string inputStr)
        {
            return new StringColDataInjector(inputStr);
        }
    }
}