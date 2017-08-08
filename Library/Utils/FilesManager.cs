using System;
using System.IO;
using System.Reflection;

namespace HtmlTable.Utils
{
    /// <summary>
    /// Simplistic file manager to retrieve the Views\Table.cshtml Razor view
    /// </summary>
    internal static class FilesManager
    {
        /// <summary>
        /// Generate the path to reach the targeted Rarzor view
        /// </summary>
        private static readonly string PathToViewsDirectory = Path.Combine(new Uri(Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase) ?? "").LocalPath, "Views");

        private static string TableRazorViewName => "Table.cshtml";

        /// <summary>
        /// Returns the full path to access the Razor view
        /// </summary>
        public static string PathToTableRazorView => File.ReadAllText($"{PathToViewsDirectory}/{TableRazorViewName}");
    }
}