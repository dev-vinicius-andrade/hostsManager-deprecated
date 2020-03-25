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
using HostsManager.Services.Helpers;


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
            _managerService.ConfigurationsChanged += ManagerServiceOnConfigurationsChanged;
            InitializeComponent();
            Width = _mainWindowConfigurations.Width;
            Height = _mainWindowConfigurations.Height;
            Background = new SolidColorBrush(_themeController.GetColor(_mainWindowConfigurations.BackgroundColor));
            Initialize();
            
        }

        private void ManagerServiceOnConfigurationsChanged(object sender, EventArgs e) => Dispatcher.Invoke(Initialize);
        public void Initialize()
        {

            LoadActiveProfile();
            LoadProfiles();
        }


        private void LoadProfiles()
        {
            GProfiles.Width = Width - (GMainGrid.Margin.Left + GMainGrid.Margin.Right);
            GProfiles.Children.Clear();
            var profiles = _managerService.GetProfiles().OrderBy(p => p.Key).ToList();

            if (!profiles.Any())
            {
                GProfiles.Visibility = Visibility.Collapsed;
                return;
            }

            GProfiles.AddElement(
                element: ElementsBuilders.BuildTextBlock("Profiles:", Helpers.Colors.WHITE, FontWeights.Bold, 24),
                column: 0,
                row: 1);
            foreach (var profile in profiles.Where(p=>!p.Key.Equals(Constants.DefaultProfileName)))
                LoadUserProfiles(profile);


            LoadDefaultProfile(profiles.FirstOrDefault(p => p.Key.Equals(Constants.DefaultProfileName)));

        }

        private void LoadDefaultProfile(KeyValuePair<string, Profile> profile)
        {
            var expander = ElementsBuilders.BuildExpander(width: GProfiles.Width,
                element: ElementsBuilders.BuildTextBlock(profile.Key, Helpers.Colors.WHITE, FontWeights.SemiBold, 16));
            var dockPanel = new DockPanel { FlowDirection = FlowDirection.LeftToRight, HorizontalAlignment = HorizontalAlignment.Left, Width = expander.Width };
            var activateButton = new Button { Tag = profile, Width = expander.Width, HorizontalAlignment = HorizontalAlignment.Left };
            activateButton.Click += ActivateButton_Click;
            activateButton.Content = new TextBlock { Text = "Activate" };
            
            dockPanel.Children.Add(activateButton);

            expander.Content = dockPanel;
            GProfiles.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            GProfiles.AddElement(expander, 0, GProfiles.RowDefinitions.Count - 1);
        }

        private void LoadUserProfiles(KeyValuePair<string,Profile> profile)
        {
            var expander = ElementsBuilders.BuildExpander(width: GProfiles.Width,
                element: ElementsBuilders.BuildTextBlock(profile.Key, Helpers.Colors.WHITE, FontWeights.SemiBold, 16));
            var dockPanel = new DockPanel { FlowDirection = FlowDirection.LeftToRight, HorizontalAlignment = HorizontalAlignment.Left, Width = expander.Width };
            var editButton = new Button { Tag = profile, Width = expander.Width / 2, HorizontalAlignment = HorizontalAlignment.Left };
            var activateButton = new Button { Tag = profile, Width = expander.Width / 2, HorizontalAlignment = HorizontalAlignment.Left };

            editButton.Content = new TextBlock { Text = "Edit" };
            editButton.Click += EditButtonOnClick;
            activateButton.Click += ActivateButton_Click;
            activateButton.Content = new TextBlock { Text = "Activate" };
            dockPanel.Children.Add(editButton);
            dockPanel.Children.Add(activateButton);

            expander.Content = dockPanel;
            GProfiles.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            GProfiles.AddElement(expander, 0, GProfiles.RowDefinitions.Count - 1);
        }
        private void EditButtonOnClick(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            var profile = (KeyValuePair<string, Profile>)button.Tag;
            var profileWindow = new ProfileWindow(_managerService, profile, _profileWindowConfigurations, _themeController){Topmost = true};
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
            GActiveProfile.Width = Width - (GMainGrid.Margin.Left + GMainGrid.Margin.Right);
            GActiveProfile.Children.Clear();
            var profile = _managerService.GetActiveProfile();
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
            GActiveProfile.AddElement(expander,0, GActiveProfile.RowDefinitions.Count - 1);
        }

        private void MainWindow_OnDeactivated(object sender, EventArgs e)
        {
            if (this.IsVisible)
                this.Hide();
        }
    }
}
