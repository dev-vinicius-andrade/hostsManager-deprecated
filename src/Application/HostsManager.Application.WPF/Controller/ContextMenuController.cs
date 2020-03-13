using System;
using System.Windows.Forms;

namespace HostsManager.Application.WPF.Controller
{
    internal class ContextMenuController : IDisposable
    {
        private readonly IDisposable _parent;
        public ContextMenuStrip ContextMenuStrip { get; }
        public ContextMenuController(IDisposable parent)
        {
            _parent = parent;
            ContextMenuStrip = new ContextMenuStrip
            {
                Items =
                {
                    ConfigureExitButton()
                }
            };
        }
        private ToolStripButton ConfigureExitButton()
        {
            var button = new ToolStripButton("Exit");
            button.Click += (sender, args) => Dispose();
            return button;
        }

        public void Dispose()
        {
            ContextMenuStrip.Dispose();
            _parent?.Dispose();
        }
    }
}
