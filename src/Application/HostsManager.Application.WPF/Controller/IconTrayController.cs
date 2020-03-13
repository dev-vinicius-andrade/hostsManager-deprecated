using System;
using System.Drawing;
using System.Windows;
using System.Windows.Forms;
using HostsManager.Application.WPF.Helpers;
using NotifyIcon = System.Windows.Forms.NotifyIcon;

namespace HostsManager.Application.WPF.Controller
{
    internal class IconTrayController:IDisposable
    {
        private readonly IDisposable _parent;
        private readonly NotifyIcon _notifyIcon;
        private readonly ContextMenuController _contextMenuController;

        public IconTrayController(IDisposable parent)
        {
            _parent = parent;
            _notifyIcon = new NotifyIcon {Icon = new Icon(Assets.GetIconImageStream())};
            _contextMenuController=new ContextMenuController(this);
        }
      
        public void Configure(Window window)
        {
            
            ConfigureWindowPosition(window);
            _notifyIcon.ContextMenuStrip = _contextMenuController.ContextMenuStrip;
            _notifyIcon.Visible = true;
            _notifyIcon.Tag = window;
            _notifyIcon.Click += NotifyIconOnClick;
            
        }
        
        public void Dispose()
        {
            if (_notifyIcon == null) return;
            
            _notifyIcon.Visible = false;
            _notifyIcon.Dispose();
            _parent?.Dispose();
        }
        private void NotifyIconOnClick(object sender, EventArgs e)
        {
            if (sender == null)
                return;
            var mouseEvent = (MouseEventArgs) e;
            var window = (Window) _notifyIcon.Tag;
            if(mouseEvent.Button== MouseButtons.Left)
                ShowHideWindow(window);
        }

        private void ShowHideWindow(Window window)
        {
            if (!window.IsVisible)
                window.Show();
            else
                window.Hide();
        }
        private void ConfigureWindowPosition(Window window)
        {
            window.Left = Screen.PrimaryScreen.WorkingArea.Right - window.Width;
            window.Top = Screen.PrimaryScreen.WorkingArea.Bottom - window.Height;
        }

   
    }
}
