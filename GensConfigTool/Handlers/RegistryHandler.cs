using ConfigurationTool.Model.Configurations;
using ConfigurationTool.Model.Settings;
using Microsoft.Win32;

namespace ConfigurationTool.Handlers
{
    class RegistryHandler
    {
        public static void FixRegistry(int fixtype)
        {
            RegistryKey registryKey = null;
            bool fixAll = false;

            switch (fixtype)
            {
                case -1:
                    registryKey = Registry.LocalMachine.CreateSubKey(RegistryConfiguration.ConfigLocation);
                    fixAll = true;
                    goto case 1;
                case 1:
                    registryKey ??= Registry.LocalMachine.OpenSubKey(RegistryConfiguration.ConfigLocation, true);
                    registryKey.SetValue(RegistryConfiguration.REGDATA_LOCALE, ((int)Language.English).ToString());
                    if (fixAll) goto case 2;
                    break;
                case 2:
                    registryKey ??= Registry.LocalMachine.OpenSubKey(RegistryConfiguration.ConfigLocation, true);
                    registryKey.SetValue(RegistryConfiguration.REGDATA_SAVELOCATION, "My Games\\Sonic Generations\\Saved Games");
                    break;
            }
            registryKey?.Close();
        }
    }
}
