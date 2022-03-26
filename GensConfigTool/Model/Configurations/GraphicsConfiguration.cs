using ConfigurationTool.Model.Devices;
using ConfigurationTool.Model.Settings;
using ConfigurationTool.Settings.Model;
using System;
using System.IO;
using System.Windows;

namespace ConfigurationTool.Model.Configurations
{
    class GraphicsConfiguration : IConfiguration
    {
        private string FileWarning => Application.Current.TryFindResource("GraphicsFile_Warning").ToString();
        public const string ConfigLocation = "GraphicsConfig.cfg";

        public Configuration LoadConfiguration(Configuration config)
        {
            if (config == null) config = new Configuration();

            if (!File.Exists(ConfigLocation)) return config;
            try
            {
                using (StreamReader sr = new StreamReader(new BufferedStream(File.Open(ConfigLocation, FileMode.Open))))
                {
                    sr.ReadLine(); // Skip warning line
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
                    };
                    config.RefreshRate = new RefreshRate(int.Parse(resString[2]));

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
                        MonitorID = sr.ReadLine()
                    };

                    config.DepthFormat = DepthFormat.FromFourCC(int.Parse(sr.ReadLine()));
                    return config;
                }
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
                writer.WriteLine(FileWarning);
                writer.WriteLine(config.GraphicsAdapter.Description);
                writer.WriteLine(config.GraphicsAdapter.Name);
                Resolution res = config.Resolution;
                writer.WriteLine($"{res.Width}.{res.Height}.{config.RefreshRate.Value}");
                writer.WriteLine((int)config.Antialiasing);
                writer.WriteLine((int)config.VSync);
                writer.WriteLine((int)config.ShadowQuality);
                writer.WriteLine((int)config.ReflectionQuality);
                writer.WriteLine((int)config.DisplayMode);
                writer.WriteLine(config.GraphicsAdapter.GUID);
                writer.WriteLine(config.GraphicsAdapter.MonitorID);
                writer.WriteLine(config.DepthFormat.GetFourCC());
            }
        }
    }
}
