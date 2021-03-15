using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;

namespace ConfigurationTool.Handlers
{
    class RegistryHandler
    {
        // English, French, German, Italian, Spanish and Japanese
        public readonly HashSet<string> VALID_LOCALES = new HashSet<string>() { "1033", "1036", "1031", "1040", "3082", "0411" };

        // English
        public const int DEFAULT_LOCALE = 1033;
        public static string DEFAULT_SAVELOCATION = "My Games\\Sonic Generations\\Saved Games";

        public static string REGDATA_DIR = "SOFTWARE\\Sega\\Sonic Generations";

        // The game boots fine without the install path and exe path
        public const string REGDATA_INSTALLPATH = "install_path";
        public const string REGDATA_EXEPATH = "exe_path";
        public const string REGDATA_LOCALE = "locale";
        public const string REGDATA_SAVELOCATION = "savelocation";

        public void LoadReg()
        {
            int fixRegistry = 0;

            // Load Registry
            RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(REGDATA_DIR);
            if (registryKey == null)
            {
                fixRegistry = -1;
                registryKey = Registry.LocalMachine.OpenSubKey(REGDATA_DIR);
            }

            // Load Locale
            object locale = registryKey?.GetValue(REGDATA_LOCALE);
            if (locale != null)
            {
                string regLocale = locale.ToString();
                if (VALID_LOCALES.Contains(regLocale)) this.LocaleID = int.Parse(regLocale);
                else this.LocaleID = DEFAULT_LOCALE;
            }
            else
            {
                if (fixRegistry >= 0) fixRegistry = 1;
                this.LocaleID = DEFAULT_LOCALE;
            }

            // Load Input Save Location
            object saveLocation = registryKey?.GetValue(REGDATA_SAVELOCATION);
            if (saveLocation == null)
            {
                if (fixRegistry >= 0) fixRegistry = 2;
                saveLocation = DEFAULT_SAVELOCATION;
            }
            this.InputSaveLocation = $"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}\\{saveLocation}";

            registryKey?.Close();
            PromptToFix(fixRegistry);
        }

        private void PromptToFix(int fixType)
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

        public void FixRegistry(int fixtype)
        {
            RegistryKey registryKey = null;
            bool fixAll = false;

            switch (fixtype)
            {
                case -1:
                    registryKey = Registry.LocalMachine.CreateSubKey(REGDATA_DIR);
                    fixAll = true;
                    goto case 1;
                case 1:
                    registryKey ??= Registry.LocalMachine.OpenSubKey(REGDATA_DIR, true);
                    registryKey.SetValue(REGDATA_LOCALE, DEFAULT_LOCALE.ToString());
                    if (fixAll) goto case 2;
                    break;
                case 2:
                    registryKey ??= Registry.LocalMachine.OpenSubKey(REGDATA_DIR, true);
                    registryKey.SetValue(REGDATA_SAVELOCATION, DEFAULT_SAVELOCATION);
                    break;
            }
            registryKey?.Close();
        }

        // I swear this has no use, but the game cares about it
        public int LocaleID { get; set; }

        // Used for Input config
        public string InputSaveLocation { get; set; }
    }
}
