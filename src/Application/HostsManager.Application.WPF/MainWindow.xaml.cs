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
        private ConfigurationsService _service;

        public MainWindow()
        {
            _service = new ConfigurationsService();
            
            InitializeComponent();
            LoadProfiles();
        }

        private void LoadProfiles()
        {

            foreach (var profile in _service.GetConfigurations().Profiles)
            {
                GActiveProfiles.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                var text = new TextBlock
                {
                    Text = profile.Key,
                    FontSize = 20

                };
                Grid.SetColumn(text, 0);
                Grid.SetRow(text, GActiveProfiles.RowDefinitions.Count - 1);
                GActiveProfiles.Children.Add(text);
            }          
   
        }
    }
}
