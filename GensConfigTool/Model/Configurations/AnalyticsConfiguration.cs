using ConfigurationTool.Settings.Model;
using System.IO;

namespace ConfigurationTool.Model.Configurations
{
    class AnalyticsConfiguration : IConfiguration
    {
        public string ConfigFile => "StatsConfig.cfg";

        public BasicConfiguration LoadConfiguration(BasicConfiguration config)
        {
            if (config == null) config = new BasicConfiguration();

            if (!File.Exists(ConfigFile)) return config;
            try
            {
                using (StreamReader sr = new StreamReader(new BufferedStream(File.Open(ConfigFile, FileMode.Open))))
                {
                    int value = int.Parse(sr.ReadLine());
                    config.Analytics = value > 0 ? OnOff.On : OnOff.Off;
                }
                return config;
            }
            catch
            {
                return new BasicConfiguration();
            }
        }

        public void SaveConfiguration(BasicConfiguration config)
        {
            using (StreamWriter writer = new StreamWriter(ConfigFile))
            {
                writer.WriteLine(config.Analytics.isOn ? 1 : 0);
            }
        }
    }
}
