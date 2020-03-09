using HostsManager.Application.Entities;
using HostsManager.Application.Helpers;
using HostsManager.Application.WPF.Helpers;
using HostsManager.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace HostsManager.Application.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ManagerServices _managerService;

        public MainWindow()
        {
            _managerService = new ManagerServices();
            Initialize();
        }
        public void Initialize()
        {
            InitializeComponent();
            LoadActiveProfile();
            LoadProfiles();
        }


        private void LoadProfiles()
        {
            GProfiles.Children.Clear();
            var profiles = _managerService.GetProfiles().OrderBy(p=>p.Key);
            
            if (!profiles.Any())
            {
                GProfiles.Visibility = Visibility.Collapsed;
                return;
            }
            var profilesLabel = new TextBlock
            {
                Text = "Profiles:",
                Foreground = new SolidColorBrush("FFFFFF".ToHexadecimalColor()),
                FontWeight = FontWeights.Bold
            };
            Grid.SetColumn(profilesLabel, 0);
            Grid.SetRow(profilesLabel, 1);
            GProfiles.Children.Add(profilesLabel);

            foreach (var profile in profiles)
            {
                var expander = new Expander()
                {
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    FlowDirection = FlowDirection.RightToLeft,
                    Width = GMainGrid.Width - 15,
                    Header = new TextBlock
                    {
                        TextAlignment = TextAlignment.Left,
                        Margin = new Thickness(0),
                        Text = profile.Key,
                        FontSize = 16,
                        Foreground = new SolidColorBrush("FFFFFF".ToHexadecimalColor())

                    }
                };
                var stackPanel = new StackPanel { FlowDirection = FlowDirection.LeftToRight};

                foreach (var host in profile.Value.Hosts)
                    stackPanel.Children.Add(new TextBlock
                    {
                        Text = $"{host.Ip}  {host.Host}",
                        FontSize = 14,
                        Foreground = new SolidColorBrush("FFFFFF".ToHexadecimalColor())

                    });

                var activateButton = new Button();
                activateButton.Tag = profile;
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
            var activeProfileLabel = new TextBlock
            {
                Text = "Active Profile:",
                Foreground = new SolidColorBrush("FFFFFF".ToHexadecimalColor()),
                FontWeight = FontWeights.Bold
            };
            Grid.SetColumn(activeProfileLabel, 0);
            Grid.SetRow(activeProfileLabel, 0);
            GActiveProfile.Children.Add(activeProfileLabel);

            var expander = new Expander()
            {
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Left,
                FlowDirection = FlowDirection.RightToLeft,
                Width = GMainGrid.Width - 15,
                Background = new SolidColorBrush("64dd17".ToHexadecimalColor()),
                Header = new TextBlock
                {
                    TextAlignment = TextAlignment.Left,
                    Margin = new Thickness(0),
                    Text = profile.Key,
                    FontSize = 16,
                    Foreground = new SolidColorBrush("000000".ToHexadecimalColor())

                }
            };
            var stackPanel = new StackPanel { FlowDirection = FlowDirection.LeftToRight };

            foreach (var host in profile.Value.Hosts)
                stackPanel.Children.Add(new TextBlock
                {
                    Text = $"{host.Ip}  {host.Host}",
                    FontSize = 14,
                    Foreground = new SolidColorBrush("000000".ToHexadecimalColor())

                });

            expander.Content = stackPanel;
            GActiveProfile.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            Grid.SetColumn(expander, 0);
            Grid.SetRow(expander, GActiveProfile.RowDefinitions.Count - 1);
            
            GActiveProfile.Children.Add(expander);
        }
    }
}
