using System.Drawing;

namespace HostsManager.Application.WPF.Helpers
{
    internal static class Extensions
    {
        public static System.Windows.Media.Color ToHexadecimalColor(this string hexadecimalColor)
        {
            var colorConverted = ColorTranslator.FromHtml(hexadecimalColor.FormatHexadecimalColor());
            return System.Windows.Media.Color.FromArgb(colorConverted.A, colorConverted.R, colorConverted.G, colorConverted.B);
        }
        public static string FormatHexadecimalColor(this string color) => color.StartsWith("#") ? color : $"#{color}";

    }
}
