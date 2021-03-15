using System.IO;

namespace ConfigurationTool.Model.Configurations
{
    class AudioConfiguration : IConfiguration
    {
        public string ConfigFile => "AudioConfig.cfg";

        public BasicConfiguration LoadConfiguration(BasicConfiguration config)
        {
            if (config == null) config = new BasicConfiguration();

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
                return new BasicConfiguration();
            }
        }

        public void SaveConfiguration(BasicConfiguration config)
        {

            using (StreamWriter writer = new StreamWriter(ConfigFile))
            {
                writer.WriteLine(config.AudioDevice.Name);
                writer.WriteLine(config.AudioDevice.GUID);
            }
        }
    }
}
