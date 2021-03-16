using ConfigurationTool.Handlers;
using ConfigurationTool.Helpers;
using ConfigurationTool.Model.Settings;
using Microsoft.Win32;
using System;
using System.Windows;

namespace ConfigurationTool.Model.Configurations
{
    class RegistryConfiguration : IConfiguration
    {
        public const string ConfigLocation = "SOFTWARE\\Sega\\Sonic Generations";

        // The game boots fine without the install path and exe path
        public const string REGDATA_INSTALLPATH = "install_path";
        public const string REGDATA_EXEPATH = "exe_path";
        public const string REGDATA_LOCALE = "locale";
        public const string REGDATA_SAVELOCATION = "savelocation";

        public Configuration LoadConfiguration(Configuration config)
        {
            int fixRegistry = 0;

            // Load Registry
            RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(ConfigLocation);
            if (registryKey == null)
            {
                fixRegistry = -1;
                registryKey = null;
            }

            // Load Locale
            object locale = registryKey?.GetValue(REGDATA_LOCALE);
            if (locale != null)
            {
                int regLocale = int.Parse(locale.ToString());
                if (Array.IndexOf(Enum.GetValues(typeof(Language)), regLocale) >= 0)
                {
                    config.Language = (Language)regLocale;
                }
            }
            else
            {
                fixRegistry = Math.Max(fixRegistry, 1);
            }

            // Load Input Save Location
            object saveLocation = registryKey?.GetValue(REGDATA_SAVELOCATION);
            if (saveLocation == null)
            {
                fixRegistry = Math.Max(fixRegistry, 2);
                saveLocation = config.InputSaveLocation;
            }
            registryKey?.Close();

            config.InputSaveLocation = $"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}\\{saveLocation}";

            TryFixRegInSameProcess(fixRegistry, config.ProcessIsElevated);
            return config;
        }

        public void SaveConfiguration(Configuration config)
        {
            if (!config.ProcessIsElevated) return;

            RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(ConfigLocation, true);
            if (registryKey != null)
            {
                registryKey.SetValue(REGDATA_LOCALE, ((int)config.Language).ToString());
                registryKey.SetValue(REGDATA_SAVELOCATION, config.InputSaveLocation);
            }
        }

        private static void TryFixRegInSameProcess(int fixType, bool isElevated)
        {
            if (fixType != 0)
            {
                if (isElevated)
                {
                    RegistryHandler.FixRegistry(fixType);
                }
                else
                {
                    if (MessageBox.Show(Application.Current.TryFindResource("RegError").ToString(),
                        Application.Current.TryFindResource("RegError_Title").ToString(),
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Warning) == MessageBoxResult.Yes)
                    {
                        UacHelper.RestartAsAdminAndWait(fixType.ToString());
                    }
                }
            }
        }
    }
}
