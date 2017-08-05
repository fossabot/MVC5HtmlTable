namespace HtmlTable.Strategies.Converters
{
    /// <summary>
    /// Allows to create implementations, used to apply custom logic to altering the input string
    /// </summary>
    public interface IColConverter
    {
        string Convert(string input);
    }
}