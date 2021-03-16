using ConfigurationTool.Model;
using ConfigurationTool.Model.Configurations;
using ConfigurationTool.Model.Settings;
using Microsoft.Win32;

namespace ConfigurationTool.Handlers
{
    class RegistryHandler
    {
        public static void FixRegistry(int fixtype)
        {
            RegistryConfiguration reg = new RegistryConfiguration();
            Configuration config = new Configuration();

            RegistryKey registryKey = null;
            bool fixAll = false;

            switch (fixtype)
            {
                case -1:
                    registryKey = Registry.LocalMachine.CreateSubKey(reg.ConfigLocation);
                    fixAll = true;
                    goto case 1;
                case 1:
                    registryKey ??= Registry.LocalMachine.OpenSubKey(reg.ConfigLocation, true);
                    registryKey.SetValue(RegistryConfiguration.REGDATA_LOCALE, ((int)Language.English).ToString());
                    if (fixAll) goto case 2;
                    break;
                case 2:
                    registryKey ??= Registry.LocalMachine.OpenSubKey(reg.ConfigLocation, true);
                    registryKey.SetValue(RegistryConfiguration.REGDATA_SAVELOCATION, config.InputSaveLocation);
                    break;
            }
            registryKey?.Close();
        }
    }
}
