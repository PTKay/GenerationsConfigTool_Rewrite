using ConfigurationTool.Handlers;
using ConfigurationTool.Model.Settings;
using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.Windows;

namespace ConfigurationTool.Model.Configurations
{
    class RegistryConfiguration : IConfiguration
    {
        public string ConfigLocation => "SOFTWARE\\Sega\\Sonic Generations";

        // The game boots fine without the install path and exe path
        public const string REGDATA_INSTALLPATH = "install_path";
        public const string REGDATA_EXEPATH = "exe_path";
        public const string REGDATA_LOCALE = "locale";
        public const string REGDATA_SAVELOCATION = "savelocation";

        public Configuration LoadConfiguration(Configuration config)
        {
            int fixRegistry;
            do 
            {
                fixRegistry = 0;

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
                    if (fixRegistry >= 0) fixRegistry = 1;
                }

                // Load Input Save Location
                object saveLocation = registryKey?.GetValue(REGDATA_SAVELOCATION);
                if (saveLocation == null)
                {
                    if (fixRegistry >= 0) fixRegistry = 2;
                    saveLocation = config.InputSaveLocation;
                }
                registryKey?.Close();

                config.InputSaveLocation = $"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}\\{saveLocation}";
            } while (TryFixRegInSameProcess(fixRegistry, config.ProcessIsElevated));

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

        private static bool TryFixRegInSameProcess(int fixType, bool isElevated)
        {
            if (fixType != 0)
            {
                if (isElevated)
                {
                    RegistryHandler.FixRegistry(fixType);
                    return true;
                }
                else
                {
                    if (MessageBox.Show(Application.Current.TryFindResource("RegError").ToString(),
                        Application.Current.TryFindResource("RegError_Title").ToString(),
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Warning) == MessageBoxResult.Yes)
                    {
                        var psi = new ProcessStartInfo
                        {
                            UseShellExecute = true,
                            FileName = Process.GetCurrentProcess().MainModule.FileName,
                            Arguments = fixType.ToString(),
                            Verb = "runas"
                        };

                        var process = new Process
                        {
                            StartInfo = psi
                        };
                        process.Start();
                        process.WaitForExit();
                        return false;
                    }
                }
            }
            return false;
        }
    }
}
