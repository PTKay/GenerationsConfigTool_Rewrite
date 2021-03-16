using ConfigurationTool.Model.Devices;
using System.IO;

namespace ConfigurationTool.Model.Configurations
{
    class AudioConfiguration : IConfiguration
    {
        public string ConfigFile => "AudioConfig.cfg";

        public Configuration LoadConfiguration(Configuration config)
        {
            if (config == null) config = new Configuration();

            if (!File.Exists(ConfigFile)) return config;
            try
            {
                using (StreamReader sr = new StreamReader(new BufferedStream(File.Open(ConfigFile, FileMode.Open))))
                {
                    string name = sr.ReadLine();
                    string guid = sr.ReadLine();
                    config.AudioDevice = new AudioDevice()
                    {
                        Name = name,
                        GUID = guid
                    };
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

            using (StreamWriter writer = new StreamWriter(ConfigFile))
            {
                writer.WriteLine(config.AudioDevice.Name);
                writer.WriteLine(config.AudioDevice.GUID);
            }
        }
    }
}
