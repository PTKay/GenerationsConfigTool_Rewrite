using ConfigurationTool.Settings.Model;
using System.IO;

namespace ConfigurationTool.Model.Configurations
{
    class AnalyticsConfiguration : IConfiguration
    {
        public string ConfigLocation => "StatsConfig.cfg";

        public Configuration LoadConfiguration(Configuration config)
        {
            if (config == null) config = new Configuration();

            if (!File.Exists(ConfigLocation)) return config;
            try
            {
                using (StreamReader sr = new StreamReader(new BufferedStream(File.Open(ConfigLocation, FileMode.Open))))
                {
                    int value = int.Parse(sr.ReadLine());
                    config.Analytics = value > 0 ? OnOff.On : OnOff.Off;
                }
                return config;
            }
            catch
            {
                return new Configuration();
            }
        }

        public void SaveConfiguration(Configuration config)
        {
            using (StreamWriter writer = new StreamWriter(ConfigLocation))
            {
                writer.WriteLine((int)config.Analytics);
            }
        }
    }
}
