namespace HtmlTableHelper.Injectors.Filters
{
    public interface IColFilter
    {
        bool FilterForValue(string value);
    }

    public class ColClassFilter
    {
        private readonly IColFilter _filter;
        private readonly string _classes;

        public ColClassFilter(IColFilter filter, string classes)
        {
            _filter = filter;
            _classes = classes;
        }

        public string GetClassIfFilterMatches(string value)
        {
            return _filter.FilterForValue(value) ? _classes : "";
        }
    }
}