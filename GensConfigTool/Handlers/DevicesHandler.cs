using ConfigurationTool.Model.Devices;
using ConfigurationTool.Settings.Model;
using SharpDX.Direct3D9;
using SharpDX.DirectSound;
using System.Collections.Generic;

namespace ConfigurationTool.Handlers
{
    class DevicesHandler
    {
        public static List<GraphicsAdapter> GetGraphicsAdapters()
        {
            List<GraphicsAdapter> toReturn = new List<GraphicsAdapter>();

            Direct3D d3d = new Direct3D();
            for (int i = 0; i < d3d.AdapterCount; i++)
            {
                AdapterInformation adapter = d3d.Adapters[i];
                GraphicsAdapter currAdapter = new GraphicsAdapter()
                {
                    Description = adapter.Details.Description,
                    Name = adapter.Details.DeviceName,
                    GUID = adapter.Details.DeviceIdentifier.ToString(),
                    Index = i
                };
                toReturn.Add(currAdapter);

                foreach (SharpDX.Direct3D9.DisplayMode mode in adapter.GetDisplayModes(adapter.CurrentDisplayMode.Format))
                {
                    currAdapter.Resolutions.Add(new Resolution()
                    {
                        Width = mode.Width,
                        Height = mode.Height,
                        Frequency = mode.RefreshRate
                    });
                }

                currAdapter.Resolutions.Sort((a, b) => b.CompareTo(a));
            }

            return toReturn;
        }

        public static IEnumerable<AudioDevice> GetAudioDevices()
        {

            List<AudioDevice> toReturn = new List<AudioDevice>
            {
                new AudioDevice(),
                new AudioDevice()
                {
                    Name = "None",
                    GUID = "00000000-0000-0000-0000-000000000000"
                }
            };
            foreach (DeviceInformation device in DirectSound.GetDevices())
            {
                toReturn.Add(new AudioDevice()
                {
                    Name = device.Description,
                    GUID = device.DriverGuid.ToString()
                });
            }
            return toReturn;
        }
    }
}
