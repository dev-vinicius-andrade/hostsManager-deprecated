
using System.Collections.Generic;
using System.Linq;


namespace HostsManager.Application.Helpers
{
    internal static class Extensions
    {

        public static List<string> SplitByEmptySpace(this string text)
             =>
                 text.Contains(CharHelper.TabChar()) ? text.Split(CharHelper.TabChar()).ToList() : text.Split(CharHelper.SpaceChar()).ToList();


    }
}
