using Microsoft.Win32;
using System;

namespace ConfigurationTool
{
    class Program
    {
        [STAThread]
        private static void Main(string[] args)
        {
            if (args.Length > 0)
            {
                FixReg(args[0]);
                return;
            }
            App.Main();
        }

        static void FixReg(string mode)
        {
            RegistryKey registryKey = null;
            bool fixAll = false;
            switch (mode)
            {
                case "-1":
                    registryKey = Registry.LocalMachine.CreateSubKey(RegistryData.REG_LOCATION);
                    fixAll = true;
                    goto case "1";
                case "1":
                    if (registryKey == null) registryKey = Registry.LocalMachine.OpenSubKey(RegistryData.REG_LOCATION, true);
                    registryKey.SetValue("locale", "1033");
                    if (fixAll) goto case "2";
                    break;
                case "2":
                    if (registryKey == null) registryKey = Registry.LocalMachine.OpenSubKey(RegistryData.REG_LOCATION, true);
                    registryKey.SetValue("SaveLocation", "My Games\\Sonic Generations\\Saved Games");
                    break;
            }
            registryKey?.Close();
        }
    }
}
