using System.IO;
using System.Reflection;
namespace HostsManager.Application.WPF.Helpers
{
    internal static class Assets
    {
        public static Stream GetIconImageStream() => Assembly.GetExecutingAssembly()
            .GetManifestResourceStream($"HostsManager.Application.WPF.Assets.icon.ico");
    }
}
