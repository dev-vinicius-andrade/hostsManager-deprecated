using System;

namespace HostsManager.Application.Helpers
{
    internal static class OsHelper
    {
        private static readonly PlatformID Platform = Environment.OSVersion.Platform;
        public static bool IsWindows() 
            => Platform == PlatformID.Win32Windows || Platform == PlatformID.Win32NT ||
                   Platform == PlatformID.Win32S || Platform == PlatformID.WinCE;


        public static bool IsMac()
            => Platform == PlatformID.MacOSX;


        public static bool IsUnix()
            => Platform == PlatformID.Unix;

    }
}
