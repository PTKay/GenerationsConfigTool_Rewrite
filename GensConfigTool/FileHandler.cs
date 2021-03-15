using ConfigurationTool.Model;
using System;
using System.IO;

namespace ConfigurationTool
{
    class FileHandler
    {
        public const string GRAPHICS_INFO = "Do not manually edit this file, use the configuration tool. Unless you know what you're doing...";

        public const string GRAPHICS_CFG = "GraphicsConfig.cfg";
        public const string AUDIO_CFG = "AudioConfig.cfg";
        public const string ANALYTICS_CFG = "StatsConfig.cfg";

        public static BasicConfiguration LoadConfiguration()
        {
            BasicConfiguration config = LoadGraphicsConfiguration();
            LoadAudioConfiguration(config);
            LoadAnalyticsConfiguration(config);
            return config;
        }

        private static BasicConfiguration LoadGraphicsConfiguration()
        {
            BasicConfiguration config = new BasicConfiguration();
            if (!File.Exists(GRAPHICS_CFG)) return config;
            try
            {
                using (StreamReader sr = new StreamReader(new BufferedStream(File.Open(GRAPHICS_CFG, FileMode.Open))))
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

                    // Forces to INTZ but we'll read it anyway
                    config.DepthFormat = DepthFormat.FromFourCC(int.Parse(sr.ReadLine()));
                    return config;
                }
            }
            catch
            {
                return new BasicConfiguration();
            }
        }

        private static void LoadAudioConfiguration(BasicConfiguration config)
        {
            if (!File.Exists(AUDIO_CFG)) return;
            try
            {
                using (StreamReader sr = new StreamReader(new BufferedStream(File.Open(AUDIO_CFG, FileMode.Open))))
                {
                    string name = sr.ReadLine();
                    string guid = sr.ReadLine();
                    config.AudioDevice = new AudioDevice()
                    {
                        Name = name,
                        GUID = guid
                    };
                }
            }
            catch { }
        }

        private static void LoadAnalyticsConfiguration(BasicConfiguration config)
        {
            if (!File.Exists(ANALYTICS_CFG)) return;
            try
            {
                using (StreamReader sr = new StreamReader(new BufferedStream(File.Open(ANALYTICS_CFG, FileMode.Open))))
                {
                    int value = int.Parse(sr.ReadLine());
                    config.Analytics = value > 0 ? OnOff.On : OnOff.Off;
                }
            }
            catch { }
        }

        public static bool SaveConfiguration(BasicConfiguration config)
        {
            try
            {
                SaveGraphicsConfiguration(config);
                SaveAudioConfiguration(config);
                SaveAnalyticsConfiguration(config);
            }
            catch
            {
                return false;
            }
            return true;
        }

        public static void SaveGraphicsConfiguration(BasicConfiguration config)
        {
            using (StreamWriter writer = new StreamWriter(GRAPHICS_CFG))
            {
                writer.WriteLine(GRAPHICS_INFO);
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

        public static void SaveAudioConfiguration(BasicConfiguration config)
        {
            using (StreamWriter writer = new StreamWriter(AUDIO_CFG))
            {
                writer.WriteLine(config.AudioDevice.Name);
                writer.WriteLine(config.AudioDevice.GUID);
            }
        }

        public static void SaveAnalyticsConfiguration(BasicConfiguration config)
        {
            using (StreamWriter writer = new StreamWriter(ANALYTICS_CFG))
            {
                writer.WriteLine(config.Analytics.isOn ? 1 : 0);
            }
        }
    }
}
