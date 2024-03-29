﻿using System.Collections.Generic;
using System.Linq;

namespace WinDevUtility
{
    internal static class PageTokens
    {
        public const string POCOPage = "Poco";
        public const string SettingsPage = "Settings";
        public const string GuidPage = "Guid";
        public const string XamlPage = "Xaml";
        public const string CommandPage = "Command";
        public const string UnusedXamlPage = "UnusedXaml";
        public const string JsonToCSharpePage = "JsonToCSharpe";
        internal static IEnumerable<string> GetAll() => typeof(PageTokens)
                                                            .GetFields()
                                                            .Select(p => p.GetValue(p) as string);

        public const string BlogsPage = "Blogs";
    }
}
