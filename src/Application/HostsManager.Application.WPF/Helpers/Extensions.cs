using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;

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
        public static void AddElement(this Grid grid, UIElement element, int column,int row)
        {
            Grid.SetColumn(element, column);
            Grid.SetRow(element, row);
            grid.Children.Add(element);
        }
        public static Dictionary<string, TValue> ConvertToCaseInSensitive<TValue>(this Dictionary<string, TValue> dictionary)
        {
            var resultDictionary = new Dictionary<string, TValue>(StringComparer.InvariantCultureIgnoreCase);
            foreach (var (key, value) in dictionary)
            {
                resultDictionary.Add(key, value);
            }

            dictionary = resultDictionary;
            return dictionary;
        }
    }
}
