using HostsManager.Application.WPF.Helpers;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using HostsManager.Services.Entities;
using HostsManager.Services.Interfaces;
using HostsManager.Application.WPF.Builders;

namespace HostsManager.Application.WPF
{
    public partial class MainWindow
    {
        private readonly IManagerService _managerService;

        public MainWindow(IManagerService managerService)
        {
             _managerService = managerService;
            Initialize();
        }
        public void Initialize()
        {
            
            InitializeComponent();
            LoadActiveProfile();
            LoadProfiles();
            Background = new SolidColorBrush(Constants.COLOR_BACKGROUND.ToHexadecimalColor());
        }


        private void LoadProfiles()
        {
            GProfiles.Children.Clear();
            var profiles = _managerService.GetProfiles().OrderBy(p => p.Key);

            if (!profiles.Any())
            {
                GProfiles.Visibility = Visibility.Collapsed;
                return;
            }
            
            GProfiles.AddElement( 
                element:ElementsBuilders.BuildTextBlock("Profiles:", Constants.COLOR_WHITE, FontWeights.Bold, 24),
                column:0,
                row:1);
            foreach (var profile in profiles)
            {
                var expander = new Expander()
                {
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    FlowDirection = FlowDirection.RightToLeft,
                    Width = GProfiles.Width,
                    Header = ElementsBuilders.BuildTextBlock(profile.Key, Constants.COLOR_WHITE, FontWeights.SemiBold, 16)
                };
                var stackPanel = new StackPanel { FlowDirection = FlowDirection.LeftToRight };
                var activateButton = new Button { Tag = profile };
                activateButton.Click += ActivateButton_Click;
                activateButton.Content = new TextBlock { Text = "Activate" };
                stackPanel.Children.Add(activateButton);

                expander.Content = stackPanel;
                GProfiles.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                Grid.SetColumn(expander, 0);
                Grid.SetRow(expander, GProfiles.RowDefinitions.Count - 1);
                GProfiles.Children.Add(expander);


            }

        }



        private void WriteProfiles(KeyValuePair<string,Profile> profile)
        {
            var stackPanel = new StackPanel();
            foreach (var host in profile.Value.Hosts)
                stackPanel.Children.Add(new TextBlock
                {
                    Text = $"{host.Ip}  {host.Host}",
                    FontSize = 14,
                    Foreground = new SolidColorBrush(Constants.COLOR_WHITE.ToHexadecimalColor())

                });
        }

        private void ActivateButton_Click(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            var profile = (KeyValuePair<string, Profile>)button.Tag;
            _managerService.ActivateProfile(profile.Key);
            LoadActiveProfile();
        }

        private void LoadActiveProfile()
        {

            GActiveProfile.Children.Clear();
            var profile = _managerService.GetActiveProfile();
            GProfiles.AddElement(
                element: ElementsBuilders.BuildTextBlock("Active Profile:", Constants.COLOR_WHITE, FontWeights.Bold, 24),
                column: 0,
                row: 1);

            var expander = new Expander()
            {
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Left,
                FlowDirection = FlowDirection.RightToLeft,
                Width = GActiveProfile.Width ,
                Background = new SolidColorBrush(Constants.COLOR_GREEN.ToHexadecimalColor()),
                Header = new TextBlock
                {
                    TextAlignment = TextAlignment.Left,
                    Margin = new Thickness(0),
                    Text = profile.Key,
                    FontSize = 16,
                    Foreground = new SolidColorBrush(Constants.COLOR_BLACK.ToHexadecimalColor())

                }
            };
            var stackPanel = new StackPanel { FlowDirection = FlowDirection.LeftToRight };

            foreach (var host in profile.Value.Hosts)
                stackPanel.Children.Add(new TextBlock
                {
                    Text = $"{host.Ip}  {host.Host}",
                    FontSize = 14,
                    Foreground = new SolidColorBrush(Constants.COLOR_BLACK.ToHexadecimalColor())

                });

            expander.Content = stackPanel;
            GActiveProfile.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            Grid.SetColumn(expander, 0);
            Grid.SetRow(expander, GActiveProfile.RowDefinitions.Count - 1);
            
            GActiveProfile.Children.Add(expander);
        }
    }
}
