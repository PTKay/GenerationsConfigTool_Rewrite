using ConfigurationTool.Helpers;
using ConfigurationTool.Model.Devices;
using ConfigurationTool.Model.Settings;
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
                    Resolution res = new Resolution()
                    {
                        Width = mode.Width,
                        Height = mode.Height
                    };
                    RefreshRate refreshRate = new RefreshRate(mode.RefreshRate);

                    int idx = currAdapter.Resolutions.IndexOf(res);
                    if (idx >= 0)
                    {
                        currAdapter.Resolutions[idx].RefreshRates.InsertElementDescending(refreshRate);
                    }
                    else
                    {
                        res.RefreshRates.Add(refreshRate);
                        currAdapter.Resolutions.InsertElementDescending(res);
                    }
                }
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
                AudioDevice toAdd = new AudioDevice()
                {
                    Name = device.Description,
                    GUID = device.DriverGuid.ToString()
                };

                // Ignore repeated devices (will also skip default output due to having the same GUID as None)
                if (!toReturn.Contains(toAdd))
                {
                    toReturn.Add(toAdd);
                }
            }
            return toReturn;
        }
    }
}
