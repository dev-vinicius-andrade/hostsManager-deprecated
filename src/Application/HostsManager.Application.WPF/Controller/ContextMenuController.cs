using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using HostsManager.Application.WPF.Configuration;
using HostsManager.Services.Interfaces;

namespace HostsManager.Application.WPF.Controller
{
    internal class ContextMenuController : IDisposable
    {

        private readonly IDisposable _parent;
        private readonly IManagerService _managerService;
        private readonly UiConfigurations _uiConfigurations;
        private readonly ThemeController _themeController;
        public ContextMenuStrip ContextMenuStrip { get; }
        public ContextMenuController(IDisposable parent, IManagerService managerService, UiConfigurations uiConfigurations, ThemeController themeController)
        {
            _parent = parent;
            _managerService = managerService;
            _uiConfigurations = uiConfigurations;
            _themeController = themeController;
            ContextMenuStrip = new ContextMenuStrip
            {
                Items =
                {
                    ResetConfigurationsButton(),
                    ReloadConfigurationsButton(),
                    ConfigureConfigurationsFolderButton(),
                    ConfigureNewProfileButton(),
                    ConfigureExitButton()
                }
            };
        }

        private ToolStripButton ResetConfigurationsButton()
        {
            var button = new ToolStripButton("Reset Configurations");
            button.Click += ResetConfigurationsButtonClick;
            return button;
        }

        private void ResetConfigurationsButtonClick(object sender, EventArgs e)=> _managerService.ResetConfigurations();

        private ToolStripButton ReloadConfigurationsButton()
        {
            var button = new ToolStripButton("Reload Configurations");
            button.Click += ReloadConfigurationsButtonClick;
            return button;
        }

        private void ReloadConfigurationsButtonClick(object sender, EventArgs e) => _managerService.NotifyConfigurationsChanged();


        private ToolStripButton ConfigureConfigurationsFolderButton()
        {
            var button = new ToolStripButton("Open Configuration Folder");
            button.Click += ConfigurationsFolderButtonClick;
            return button;
        }

        private void ConfigurationsFolderButtonClick(object sender, EventArgs e)
        {
            var configurationsFileFolder = _managerService.GetConfigurationsFolder();
            if (Directory.Exists(configurationsFileFolder))
                Process.Start(new ProcessStartInfo(configurationsFileFolder) { Verb = "open", UseShellExecute = true });
        }

        private ToolStripButton ConfigureExitButton()
        {
            var button = new ToolStripButton("Exit");
            button.Click += (sender, args) => Dispose();
            return button;
        }
        private ToolStripButton ConfigureNewProfileButton()
        {
            var button = new ToolStripButton("New Profile");
            button.Click += (sender, args) =>
            {
                var profileWindow = new ProfileWindow(_managerService, null, _uiConfigurations.ProfileWindowConfigurations, _themeController)
                {
                    Topmost = true
                };
                profileWindow.Show();
            };

            return button;
        }

        public void Dispose()
        {
            ContextMenuStrip.Dispose();
            _parent?.Dispose();
        }
    }
}
