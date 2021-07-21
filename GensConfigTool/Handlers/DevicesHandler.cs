using ConfigurationTool.Helpers;
using ConfigurationTool.Model.Devices;
using ConfigurationTool.Model.Settings;
using ConfigurationTool.Settings.Model;
using SharpDX.Direct3D9;
using SharpDX.DirectSound;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static ConfigurationTool.Helpers.GraphicsDeviceTypes;

namespace ConfigurationTool.Handlers
{
    class DevicesHandler
    {
        public static List<GraphicsAdapter> GetGraphicsAdapters()
        {
            List<GraphicsAdapter> toReturn = new List<GraphicsAdapter>();

            Direct3D d3d = new Direct3D();
            foreach (AdapterInformation adapter in d3d.Adapters)
            {
                GraphicsAdapter currAdapter = new GraphicsAdapter()
                {
                    Name = adapter.Details.DeviceName,
                    Description = adapter.Details.Description,
                    GUID = adapter.Details.DeviceIdentifier.ToString(),
                    Index = adapter.Adapter
                };

                /*
                // Get the monitor ID
                var monitor = new DISPLAYDEV();
                monitor.cb = Marshal.SizeOf(monitor);
                EnumDisplayDevices(currAdapter.Name, 0, ref monitor, 0);

                if (!string.IsNullOrEmpty(monitor.DeviceName))
                {
                    currAdapter.MonitorID = monitor.DeviceID;
                }
                */

                toReturn.InsertElementAscending(currAdapter);

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
