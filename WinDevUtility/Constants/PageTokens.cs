using System.Collections.Generic;
using System.Linq;

namespace WinDevUtility
{
    internal static class PageTokens
    {
        public const string POCOPage = "Poco";
        public const string SettingsPage = "Settings";

        internal static IEnumerable<string> GetAll() => typeof(PageTokens)
                                                            .GetFields()
                                                            .Select(p => p.GetValue(p) as string);
    }

}
