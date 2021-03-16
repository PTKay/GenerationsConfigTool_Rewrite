using ConfigurationTool.Model.Devices;
using System.IO;

namespace ConfigurationTool.Model.Configurations
{
    class InputConfiguration : IConfiguration
    {
        public string ConfigLocation => "PlayerInput.cfg";
        public string DefaultConfig => "DefaultConfig.cfg";

        public Configuration LoadConfiguration(Configuration config)
        {
            if (config == null) config = new Configuration();

            if (!File.Exists($"{config.InputSaveLocation}\\{ConfigLocation}")) return config;
            try
            {
                using (StreamReader sr = new StreamReader(new BufferedStream(File.Open($"{config.InputSaveLocation}\\{ConfigLocation}", FileMode.Open))))
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
            using (StreamWriter writer = new StreamWriter($"{config.InputSaveLocation}\\{ConfigLocation}"))
            {
                writer.Write(config.Keyboard.Serialize());
            }
        }
    }
}
