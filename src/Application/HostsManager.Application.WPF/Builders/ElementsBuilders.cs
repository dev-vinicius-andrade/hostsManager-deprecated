using HostsManager.Application.WPF.Helpers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace HostsManager.Application.WPF.Builders
{
    internal static class ElementsBuilders
    {
        public static TextBlock BuildTextBlock(string text,
            string hexadecimalColorString,
            FontWeight fontWeight,
            int fontSize,
            TextAlignment textAlignment= TextAlignment.Left,
            double margin = 0 
            )
        {
            return new TextBlock
            {
                TextAlignment = textAlignment, 
                Margin = new Thickness(margin),
                Text = text,
                Foreground = new SolidColorBrush(hexadecimalColorString.ToHexadecimalColor()),
                FontWeight = fontWeight,
                FontSize = fontSize
            };

        }
        public static Expander BuildExpander(double width,
            UIElement element,
            VerticalAlignment verticalAlignment= VerticalAlignment.Center,
            HorizontalAlignment horizontalAlignment= HorizontalAlignment.Left,
            FlowDirection flowDirection = FlowDirection.RightToLeft
            )
        {
            return new Expander{
                VerticalAlignment = verticalAlignment,
                HorizontalAlignment = horizontalAlignment,
                FlowDirection = FlowDirection.RightToLeft,
                Width = width,
                Header = element
            };
        }
    }
}
