using ConfigurationTool.Settings.Model;
using System;
using System.IO;

namespace ConfigurationTool.Model.Configurations
{
    class GraphicsConfiguration : IConfiguration
    {
        public string ConfigFile => "GraphicsConfig.cfg";

        public BasicConfiguration LoadConfiguration(BasicConfiguration config)
        {
            if (config == null) config = new BasicConfiguration();

            if (!File.Exists(ConfigFile)) return config;
            try
            {
                using (StreamReader sr = new StreamReader(new BufferedStream(File.Open(ConfigFile, FileMode.Open))))
                {
                    sr.ReadLine(); // Skip first line
                    string adapterDesc = sr.ReadLine();
                    string adapterName = sr.ReadLine();
                    if (String.IsNullOrEmpty(adapterName) || String.IsNullOrEmpty(adapterName))
                    {
                        return config;
                    }
                    string[] resString = sr.ReadLine().Split('.');
                    config.Resolution = new Resolution()
                    {
                        Width = int.Parse(resString[0]),
                        Height = int.Parse(resString[1]),
                        Frequency = int.Parse(resString[2])
                    };

                    config.Antialiasing = int.Parse(sr.ReadLine()) > 0 ? OnOff.On : OnOff.Off;
                    config.VSync = int.Parse(sr.ReadLine()) > 0 ? OnOff.On : OnOff.Off;

                    config.ShadowQuality = int.Parse(sr.ReadLine()) > 0 ? HighLow.High : HighLow.Low;
                    config.ReflectionQuality = int.Parse(sr.ReadLine()) > 0 ? HighLow.High : HighLow.Low;

                    config.DisplayMode = int.Parse(sr.ReadLine()) > 0 ? DisplayMode.Letterbox : DisplayMode.Widescreen;

                    config.GraphicsAdapter = new GraphicsAdapter()
                    {
                        GUID = sr.ReadLine(),
                        Name = adapterName,
                        Description = adapterDesc,
                    };
                    sr.ReadLine(); // Skip unused monitor line

                    config.DepthFormat = DepthFormat.FromFourCC(int.Parse(sr.ReadLine()));
                    return config;
                }
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
                writer.WriteLine(ConfigFile);
                writer.WriteLine(config.GraphicsAdapter.Description);
                writer.WriteLine(config.GraphicsAdapter.Name);
                Resolution res = config.Resolution;
                writer.WriteLine($"{res.Width}.{res.Height}.{res.Frequency}");
                writer.WriteLine(config.Antialiasing.isOn ? 1 : 0);
                writer.WriteLine(config.VSync.isOn ? 1 : 0);
                writer.WriteLine(config.ShadowQuality == HighLow.High ? 1 : 0);
                writer.WriteLine(config.ReflectionQuality == HighLow.High ? 1 : 0);
                writer.WriteLine(config.DisplayMode == DisplayMode.Letterbox ? 1 : 0);
                writer.WriteLine(config.GraphicsAdapter.GUID);
                writer.WriteLine(""); // Empty line
                writer.WriteLine(config.DepthFormat.GetFourCC());
            }
        }
    }
}
