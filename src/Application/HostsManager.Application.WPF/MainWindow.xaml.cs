using System;
using HostsManager.Application.WPF.Helpers;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using HostsManager.Services.Entities;
using HostsManager.Services.Interfaces;
using HostsManager.Application.WPF.Builders;
using HostsManager.Application.WPF.Configuration;
using HostsManager.Application.WPF.Controller;


namespace HostsManager.Application.WPF
{
    public partial class MainWindow
    {
        private readonly IManagerService _managerService;
        private readonly ThemeController _themeController;
        private readonly MainWindowConfigurations _mainWindowConfigurations;
        private readonly ProfileWindowConfigurations _profileWindowConfigurations;

        public MainWindow(IManagerService managerService, ThemeController themeController, MainWindowConfigurations mainWindowConfigurations, ProfileWindowConfigurations profileWindowConfigurations)
        {
            _managerService = managerService;
            _themeController = themeController;
            _mainWindowConfigurations = mainWindowConfigurations;
            _profileWindowConfigurations = profileWindowConfigurations;
            Initialize();
        }
        public void Initialize()
        {

            InitializeComponent();
            LoadActiveProfile();
            LoadProfiles();
            Background = new SolidColorBrush(Helpers.Colors.BACKGROUND.ToHexadecimalColor());
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
                element: ElementsBuilders.BuildTextBlock("Profiles:", Helpers.Colors.WHITE, FontWeights.Bold, 24),
                column: 0,
                row: 1);
            foreach (var profile in profiles)
            {
                var expander = ElementsBuilders.BuildExpander(width: GProfiles.Width,
                    element: ElementsBuilders.BuildTextBlock(profile.Key, Helpers.Colors.WHITE, FontWeights.SemiBold, 16));
                var stackPanel = new StackPanel { FlowDirection = FlowDirection.LeftToRight };
                var editButton = new Button { Tag = profile };
                var activateButton = new Button { Tag = profile };

                editButton.Content = new TextBlock { Text = "Edit" };
                editButton.Click += EditButtonOnClick;
                activateButton.Click += ActivateButton_Click;
                activateButton.Content = new TextBlock { Text = "Activate" };
                stackPanel.Children.Add(editButton);
                stackPanel.Children.Add(activateButton);

                expander.Content = stackPanel;
                GProfiles.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                GProfiles.AddElement(expander, 0, GProfiles.RowDefinitions.Count - 1);
            }

        }

        private void EditButtonOnClick(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            var profile = (KeyValuePair<string, Profile>)button.Tag;
            var profileWindow = new ProfileWindow(profile, _profileWindowConfigurations, _themeController);
            this.Hide();
            profileWindow.ShowDialog();
            this.Show();
            profileWindow.Dispose();
        }


        private void WriteProfiles(KeyValuePair<string, Profile> profile)
        {
            var stackPanel = new StackPanel();
            foreach (var host in profile.Value.Hosts)
                stackPanel.Children.Add(new TextBlock
                {
                    Text = $"{host.Ip}  {host.Host}",
                    FontSize = 14,
                    Foreground = new SolidColorBrush(Helpers.Colors.WHITE.ToHexadecimalColor())

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
                element: ElementsBuilders.BuildTextBlock("Active ProfilePage:", Helpers.Colors.WHITE, FontWeights.Bold, 24),
                column: 0,
                row: 1);

            var expander = new Expander()
            {
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Left,
                FlowDirection = FlowDirection.RightToLeft,
                Width = GActiveProfile.Width,
                Background = new SolidColorBrush(Helpers.Colors.GREEN.ToHexadecimalColor()),
                Header = new TextBlock
                {
                    TextAlignment = TextAlignment.Left,
                    Margin = new Thickness(0),
                    Text = profile.Key,
                    FontSize = 16,
                    Foreground = new SolidColorBrush(Helpers.Colors.BLACK.ToHexadecimalColor())

                }
            };
            var stackPanel = new StackPanel { FlowDirection = FlowDirection.LeftToRight };

            foreach (var host in profile.Value.Hosts)
                stackPanel.Children.Add(new TextBlock
                {
                    Text = $"{host.Ip}  {host.Host}",
                    FontSize = 14,
                    Foreground = new SolidColorBrush(Helpers.Colors.BLACK.ToHexadecimalColor())

                });

            expander.Content = stackPanel;
            GActiveProfile.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            Grid.SetColumn(expander, 0);
            Grid.SetRow(expander, GActiveProfile.RowDefinitions.Count - 1);

            GActiveProfile.Children.Add(expander);
        }

        private void MainWindow_OnDeactivated(object? sender, EventArgs e)
        {
            if (this.IsVisible)
                this.Hide();
        }
    }
}
