using HtmlTable.Models;

namespace HtmlTable.Strategies.Injectors.Predefined
{
    /// <summary>
    /// Basically a string wraper that implements <see cref="IColDataInjector"/>
    /// </summary>
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

        /// <summary>
        /// Implicitly converts a string to and <see cref="StringColDataInjector"/>
        /// </summary>
        /// <param name="inputStr"></param>
        public static implicit operator StringColDataInjector(string inputStr)
        {
            return new StringColDataInjector(inputStr);
        }
    }
}