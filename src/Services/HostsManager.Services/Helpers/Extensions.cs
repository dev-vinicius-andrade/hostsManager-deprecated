using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace HostsManager.Services.Helpers
{
    public static class Extensions
    {

        public static List<string> SplitByEmptySpace(this string text)
        {
            var findValue = CharHelper.SpaceChar();
            if (text.Contains(CharHelper.TabChar()))
                findValue = CharHelper.TabChar();

            var list = new List<string>();
            var firstIndexOfFindValue = text.IndexOf(findValue);
            var lastIndexOfFindValue = text.LastIndexOf(findValue);

            list.Add(text.Substring(0, firstIndexOfFindValue));
            list.Add(text.Substring(lastIndexOfFindValue+1, text.Length-lastIndexOfFindValue-1));

            return list;
        }
        public static SortedDictionary<T1, T2> ToSortedDictionary<T1, T2>(this IEnumerable<T2> source, Func<T2, T1> keySelector)
        {
            return new SortedDictionary<T1, T2>(source.ToDictionary(keySelector));
        }


        public static string ToJsonObject(this object obj)
        {
            return JsonSerializer.Serialize(obj, new JsonSerializerOptions{WriteIndented = true});
        }

    }
}
