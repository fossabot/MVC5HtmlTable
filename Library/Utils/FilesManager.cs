﻿using System;
using System.IO;
using System.Reflection;

namespace HtmlTable.Utils
{
    internal static class FilesManager
    {
        private static readonly string ViewsPath = Path.Combine(new Uri(Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase) ?? "").LocalPath, "Views");
        public static string TableView => File.ReadAllText($"{ViewsPath}/Table.cshtml");
    }
}