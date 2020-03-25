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
        private readonly KeyValuePair<string, Profile>? _profile;
        private readonly ProfileWindowConfigurations _profileWindowConfigurations;
        private readonly ThemeController _themeController;
        private TextBox _headerTextBox;
        private TextBox _profileEditorTextBox;
        private Button _saveButton;
        private Button _cancelButton;
        private bool _isNewProfile;
        private Button _deleteButton;

        public ProfileWindow(IManagerService managerService, KeyValuePair<string, Profile>? profile, ProfileWindowConfigurations profileWindowConfigurations, ThemeController themeController)
        {
            _isNewProfile = false;
            _managerService = managerService;
            _profile = profile ?? CreateExampleProfile();
            _profileWindowConfigurations = profileWindowConfigurations;
            _themeController = themeController;
            InitializeComponent();
            Initialize();
        }

        private KeyValuePair<string, Profile> CreateExampleProfile()
        {
            _isNewProfile = true;
            return new KeyValuePair<string, Profile>("TYPE THE PROFILE NAME HERE", new Profile
            {
                Active = true,

                Hosts = new List<Hosts>
                {
                    new Hosts
                    {
                        Active = true,
                        Host = "localhostexample.com",
                        Ip = "127.0.0.1"
                    }
                }
            });

        }

        private void Initialize()
        {
            Width = _profileWindowConfigurations.Width;
            Height = _profileWindowConfigurations.Height;
            Background = new SolidColorBrush(_themeController.GetColor(_profileWindowConfigurations.BackgroundColor));
            _headerTextBox = CreateAndLoadProfileNameTextBox();
            _profileEditorTextBox = CreateAndLoadEditTextBox();
            _saveButton = CreateSaveButton();
            _cancelButton = CreateCancelButton();
            _deleteButton = CreateDeleteButton();
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
        private Button CreateDeleteButton()
        {
            var button = new Button
            {
                Width = _profileWindowConfigurations.Width,
                Background = new SolidColorBrush(_themeController.GetColor(_profileWindowConfigurations.DeleteButtonConfiguration.BackgroundColor)),
                Content = new TextBlock
                {
                    Text = "Remove Profile",
                    Foreground = new SolidColorBrush(_themeController.GetColor(_profileWindowConfigurations.DeleteButtonConfiguration.FontColor)),
                    FontWeight = FontWeights.Regular,
                    FontSize = _profileWindowConfigurations.CancelButtonConfiguration.FontSize
                }
                
            };
            button.Click += DeleteButtonOnClick;
            GProfile.RowDefinitions.Add(new RowDefinition { Height = new GridLength(_profileWindowConfigurations.DeleteButtonConfiguration.Height) });
            GProfile.AddElement(button, 0, GProfile.RowDefinitions.Count - 1);
            return button;
        }
        private void DeleteButtonOnClick(object sender, RoutedEventArgs e)
        {
            if(_isNewProfile) return;
            if(_managerService.DeleteProfile(_profile.Value.Key))
                Close();
        }
        private void SaveButtonOnClick(object sender, RoutedEventArgs e)
        {

            if (_isNewProfile)
                SaveNewProfile();
            else
                SaveEditedProfile();

            Close();
        }

        private void SaveNewProfile()
        {
            if (!_profile.HasValue) return;
            _profile.Value.Value.Active = false;
            _managerService.AddProfile(_profile.Value.Key, _profile.Value.Value);
        }

        private void SaveEditedProfile()
        {
            var editedProfile = new KeyValuePair<string, Profile>(
                key: _profile?.Key,
                value: JsonSerializer.Deserialize<Profile>(_profileEditorTextBox.Text)
            );

            if (_profile.Equals(editedProfile))
                Close();

            if (_managerService.SaveProfile(editedProfile.Key, editedProfile.Value))
                Close();
        }
        private void CancelButtonOnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private TextBox CreateAndLoadEditTextBox()
        {
            if (_profile == null) return null;

            var profile = _profile.Value;
            var stackPanel = new StackPanel { FlowDirection = FlowDirection.LeftToRight };
            var profileInfoLabel = new TextBlock
            {
                Text = $"Profile Info:",
                Foreground = new SolidColorBrush(_themeController.GetColor(_profileWindowConfigurations.ProfileEditorLabelConfiguration.FontColor)),
                FontWeight = FontWeights.Bold,
                FontSize = _profileWindowConfigurations.ProfileEditorLabelConfiguration.FontSize,
                Height = _profileWindowConfigurations.ProfileEditorLabelConfiguration.Height

            };

            var textBox = new TextBox
            {
                Name = "TbProfileEditor",
                TextAlignment = TextAlignment.Left,
                Text = profile.Value.ToJsonObject(),
                Background = new SolidColorBrush(_themeController.GetColor(_profileWindowConfigurations.ProfileEditorTextBoxConfiguration.BackgroundColor)),
                Foreground = new SolidColorBrush(_themeController.GetColor(_profileWindowConfigurations.ProfileEditorTextBoxConfiguration.FontColor)),
                FontWeight = FontWeights.Medium,
                FontSize = _profileWindowConfigurations.ProfileEditorTextBoxConfiguration.FontSize,
                Margin = Margin = new Thickness(0, 10, 0, 0),
                Height = _profileWindowConfigurations.ProfileEditorTextBoxConfiguration.Height
            };

            stackPanel.Children.Add(profileInfoLabel);
            stackPanel.Children.Add(textBox);
            GProfile.RowDefinitions.Add(new RowDefinition { Height = new GridLength(_profileWindowConfigurations.ProfileEditorTextBoxConfiguration.Height) });
            GProfile.AddElement(element: stackPanel, 0, GProfile.RowDefinitions.Count - 1);
            return textBox;

        }

        private TextBox CreateAndLoadProfileNameTextBox()
        {
            if (_profile == null) return null;
            var profileName = _profile?.Key;
            var stackPanel = new StackPanel { FlowDirection = FlowDirection.LeftToRight };
            var headerTextBlockLabel = new TextBlock
            {
                Text = $"Profile Name:",
                Foreground = new SolidColorBrush(_themeController.GetColor(_profileWindowConfigurations.HeaderProfileNameLabelConfiguration.FontColor)),
                FontWeight = FontWeights.Bold,
                FontSize = _profileWindowConfigurations.HeaderProfileNameLabelConfiguration.FontSize,

            };

            var headerTextBox = new TextBox
            {
                Name = "TbProfileName",
                TextAlignment = TextAlignment.Left,
                Text = profileName,
                Background = new SolidColorBrush(_themeController.GetColor(_profileWindowConfigurations.HeaderProfileNameTextBoxConfiguration.BackgroundColor)),
                Foreground = new SolidColorBrush(_themeController.GetColor(_profileWindowConfigurations.HeaderProfileNameTextBoxConfiguration.FontColor)),
                FontWeight = FontWeights.Medium,
                FontSize = _profileWindowConfigurations.HeaderProfileNameTextBoxConfiguration.FontSize,
                Margin = new Thickness(0, 10, 0, 0),
                Height = _profileWindowConfigurations.HeaderProfileNameTextBoxConfiguration.Height

            };
            stackPanel.Children.Add(headerTextBlockLabel);
            stackPanel.Children.Add(headerTextBox);

            GProfile.RowDefinitions.Add(new RowDefinition
            {
                Height = new GridLength(_profileWindowConfigurations.HeaderProfileNameLabelConfiguration.Height
                                                                                    + _profileWindowConfigurations.HeaderProfileNameTextBoxConfiguration.Height)
            });
            GProfile.AddElement(
                element: stackPanel,
                0, GProfile.RowDefinitions.Count - 1
            );
            return headerTextBox;
        }

        public void Dispose()
        {
            _headerTextBox = null;
            _profileEditorTextBox = null;
            _saveButton = null;
            _cancelButton = null;
            this.Close();
        }
    }
}
