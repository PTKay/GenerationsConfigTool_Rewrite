using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows;

namespace ConfigurationTool
{
    class RegistryData
    {
        public readonly HashSet<string> VALID_LOCALES = new HashSet<string>() { "1033", "1040", "1036", "1031", "3082" };
        public static string REG_LOCATION = "SOFTWARE\\Wow6432Node\\Sega\\Sonic Generations";

        public RegistryData()
        {
            int fixRegistry = 0;
            RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(REG_LOCATION);

            if (registryKey == null)
            {
                fixRegistry = -1;
                registryKey = Registry.LocalMachine.OpenSubKey(REG_LOCATION);
            }

            object locale = registryKey?.GetValue("locale");
            if (locale != null)
            {
                string regLocale = locale.ToString();
                if (VALID_LOCALES.Contains(regLocale))
                {
                    this.LCID = int.Parse(regLocale);
                }
                else
                {
                    this.LCID = 1033;
                }
            }
            else
            {
                if (fixRegistry >= 0) fixRegistry = 1;
                this.LCID = 1033;
            }

            // For input configuration
            object saveLocation = registryKey?.GetValue("SaveLocation");
            if (saveLocation == null)
            {
                if (fixRegistry >= 0) fixRegistry = 2;
                saveLocation = "My Games\\Sonic Generations\\Saved Games";
            }

            StringBuilder stringBuilder = new StringBuilder(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments));

            stringBuilder.Insert(stringBuilder.Length, "\\");
            stringBuilder.Insert(stringBuilder.Length, saveLocation.ToString());
            this.ReadSaveLocationFromRegistry = stringBuilder.ToString();

            FixRegistry(fixRegistry);
        }

        private void FixRegistry(int fixType)
        {
            if (fixType != 0)
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
                }
            }
        }

        // I swear this has no use, but the game cares about it
        public int LCID { get; }

        // Used for Input config
        public string ReadSaveLocationFromRegistry { get; }
    }
}
