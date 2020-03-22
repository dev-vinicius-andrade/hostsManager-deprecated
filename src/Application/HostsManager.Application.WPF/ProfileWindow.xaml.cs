using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using HostsManager.Application.WPF.Configuration;
using HostsManager.Application.WPF.Controller;
using HostsManager.Application.WPF.Helpers;
using HostsManager.Services.Entities;
using HostsManager.Services.Helpers;
using HostsManager.Services.Interfaces;

namespace HostsManager.Application.WPF
{
    /// <summary>
    /// Interaction logic for ProfileWindow.xaml
    /// </summary>
    public partial class ProfileWindow : Window, IDisposable
    {
        private readonly IManagerService _managerService;
        private readonly KeyValuePair<string, Profile> _profile;
        private readonly ProfileWindowConfigurations _profileWindowConfigurations;
        private readonly ThemeController _themeController;
        private TextBlock _headerTextBlock;
        private TextBox _profileEditorTextBox;
        private Button _saveButton;
        private Button _cancelButton;

        public ProfileWindow(IManagerService managerService, KeyValuePair<string, Profile> profile, ProfileWindowConfigurations profileWindowConfigurations, ThemeController themeController)
        {
            _managerService = managerService;
            _profile = profile;
            _profileWindowConfigurations = profileWindowConfigurations;
            _themeController = themeController;
            InitializeComponent();
            Initialize();
        }

        private void Initialize()
        {
            Width = _profileWindowConfigurations.Width;
            Height = _profileWindowConfigurations.Height;
            Background = new SolidColorBrush(_themeController.GetColor(_profileWindowConfigurations.BackgroundColor));
            _headerTextBlock = CreateHeader();
            _profileEditorTextBox = CreateAndLoadEditTextBox();
            _saveButton = CreateSaveButton();
            _cancelButton = CreateCancelButton();
        }

        private Button CreateSaveButton()
        {
            var button = new Button
            {
                Width = _profileWindowConfigurations.Width,
                Content = new TextBlock
                {
                    Text = "Save",
                    Foreground = new SolidColorBrush(_themeController.GetColor(_profileWindowConfigurations.SaveButtonConfiguration.FontColor)),
                    FontWeight = FontWeights.Regular,
                    FontSize = _profileWindowConfigurations.SaveButtonConfiguration.FontSize
                }
            };
            button.Click += SaveButtonOnClick;
            GProfile.RowDefinitions.Add(new RowDefinition { Height = new GridLength(_profileWindowConfigurations.SaveButtonConfiguration.Height) });
            GProfile.AddElement(button, 0, GProfile.RowDefinitions.Count - 1);
            return button;
        }
        private Button CreateCancelButton()
        {

            var button = new Button
            {
                Width = _profileWindowConfigurations.Width,
                Background = new SolidColorBrush(_themeController.GetColor(_profileWindowConfigurations.CancelButtonConfiguration.BackgroundColor)),
                Content = new TextBlock
                {
                    Text = "Cancel",
                    Foreground = new SolidColorBrush(_themeController.GetColor(_profileWindowConfigurations.CancelButtonConfiguration.FontColor)),
                    FontWeight = FontWeights.Regular,
                    FontSize = _profileWindowConfigurations.CancelButtonConfiguration.FontSize
                }
            };
            button.Click += CancelButtonOnClick;
            GProfile.RowDefinitions.Add(new RowDefinition { Height = new GridLength(_profileWindowConfigurations.CancelButtonConfiguration.Height) });
            GProfile.AddElement(button, 0, GProfile.RowDefinitions.Count - 1);
            return button;
        }

        private void SaveButtonOnClick(object sender, RoutedEventArgs e)
        {

            var editedProfile = new KeyValuePair<string, Profile>(
                key: _profile.Key,
                value: JsonSerializer.Deserialize<Profile>(_profileEditorTextBox.Text)
            );

            if (_profile.Equals(editedProfile))
                Close();

            if(_managerService.SaveProfile(editedProfile.Key, editedProfile.Value))
                Close();

        }
        private void CancelButtonOnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private TextBox CreateAndLoadEditTextBox()
        {
            var textBox = new TextBox
            {
                Name = "TbProfileEditor",
                TextAlignment = TextAlignment.Left,
                Text = _profile.Value.ToJsonObject(),
                Background = new SolidColorBrush(_themeController.GetColor(_profileWindowConfigurations.TextboxConfiguration.BackgroundColor)),
                Foreground = new SolidColorBrush(_themeController.GetColor(_profileWindowConfigurations.TextboxConfiguration.FontColor)),
                FontWeight = FontWeights.Medium,
                FontSize = _profileWindowConfigurations.TextboxConfiguration.FontSize,
            };
            GProfile.RowDefinitions.Add(new RowDefinition { Height = new GridLength(_profileWindowConfigurations.TextboxConfiguration.Height) });
            GProfile.AddElement(element: textBox, 0, GProfile.RowDefinitions.Count - 1);
            return textBox;
        }

        private TextBlock CreateHeader()
        {
            var headerTextBlock = new TextBlock
            {
                Text = $"Profile: [{_profile.Key}]",
                Foreground = new SolidColorBrush(_themeController.GetColor(_profileWindowConfigurations.HeaderConfiguration.FontColor)),
                FontWeight = FontWeights.Bold,
                FontSize = _profileWindowConfigurations.HeaderConfiguration.FontSize,

            };
            GProfile.RowDefinitions.Add(new RowDefinition { Height = new GridLength(_profileWindowConfigurations.HeaderConfiguration.Height) });
            GProfile.AddElement(
                element: headerTextBlock,
                0, GProfile.RowDefinitions.Count - 1
            );
            return headerTextBlock;
        }

        public void Dispose()
        {
            _headerTextBlock = null;
            _profileEditorTextBox = null;
            _saveButton = null;
            _cancelButton = null;
            this.Close();
        }
    }
}
