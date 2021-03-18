using Microsoft.Win32;
using System;
using System.Globalization;
using System.Management;
using System.Security.Principal;

namespace ConfigurationTool.Helpers
{
    class ThemeHelper
    {
        private const string RegistryKeyPath = @"Software\Microsoft\Windows\CurrentVersion\Themes\Personalize";
        private const string RegistryValueName = "AppsUseLightTheme";
        private readonly string Query = String.Format(
                CultureInfo.InvariantCulture,
                @"SELECT * FROM RegistryValueChangeEvent WHERE Hive = 'HKEY_USERS' AND KeyPath = '{0}\\{1}' AND ValueName = '{2}'",
                WindowsIdentity.GetCurrent().User.Value,
                RegistryKeyPath.Replace(@"\", @"\\"),
                RegistryValueName);

        public enum WindowsTheme
        {
            Light,
            Dark
        }

        private readonly Action<WindowsTheme> ThemeListener;

        public ThemeHelper(Action<WindowsTheme> themeListener)
        {
            this.ThemeListener = themeListener;
        }

        public void WatchTheme()
        {
            try
            {
                var watcher = new ManagementEventWatcher(Query);

                watcher.EventArrived += (sender, args) =>
                {
                    ThemeListener(GetWindowsTheme());
                };

                watcher.Start();
            }
            catch
            {
            }

            ThemeListener(GetWindowsTheme());
        }

        private static WindowsTheme GetWindowsTheme()
        {
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(RegistryKeyPath))
            {
                object registryValueObject = key?.GetValue(RegistryValueName);
                if (registryValueObject == null)
                {
                    return WindowsTheme.Light;
                }

                int registryValue = (int)registryValueObject;

                return registryValue > 0 ? WindowsTheme.Light : WindowsTheme.Dark;
            }
        }
    }
}
