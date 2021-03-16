using ConfigurationTool.Model.Devices;
using System.IO;

namespace ConfigurationTool.Model.Configurations
{
    class InputConfiguration : IConfiguration
    {
        public string ConfigFile => "PlayerInput.cfg";
        public string DefaultConfig => "DefaultConfig.cfg";
        private readonly string ConfigFile_Location;

        public InputConfiguration(string location)
        {
            ConfigFile_Location = location;
        }

        public Configuration LoadConfiguration(Configuration config)
        {
            if (config == null) config = new Configuration();

            if (!File.Exists($"{ConfigFile_Location}\\{ConfigFile}")) return config;
            try
            {
                using (StreamReader sr = new StreamReader(new BufferedStream(File.Open($"{ConfigFile_Location}\\{ConfigFile}", FileMode.Open))))
                {
                    string name = sr.ReadLine();
                    string serialized = sr.ReadLine();
                    config.Keyboard = Keyboard.DeSerialize(serialized);
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
            using (StreamWriter writer = new StreamWriter($"{ConfigFile_Location}\\{ConfigFile}"))
            {
                writer.Write(config.Keyboard.Serialize());
            }
        }
    }
}
