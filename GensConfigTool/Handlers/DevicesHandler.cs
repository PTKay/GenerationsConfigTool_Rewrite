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

            DISPLAYDEV device = new DISPLAYDEV();
            device.cb = Marshal.SizeOf(device);
            for (uint id = 0; EnumDisplayDevices(null, id, ref device, 0); id++)
            {
                var monitor = new DISPLAYDEV
                {
                    cb = device.cb
                };
                EnumDisplayDevices(device.DeviceName, 0, ref monitor, 0);

                if (!string.IsNullOrEmpty(monitor.DeviceName))
                {
                    GraphicsAdapter currAdapter = new GraphicsAdapter()
                    {
                        Description = device.DeviceString,
                        Name = device.DeviceName,
                        MonitorID = monitor.DeviceID
                    };

                    toReturn.InsertElementDescending(currAdapter);

                    DeviceMode mode = new DeviceMode();
                    mode.dmSize = (short)Marshal.SizeOf(mode);

                    for (int i = 0; EnumDisplaySettings(device.DeviceName, i, ref mode); i++)
                    {
                        Resolution res = new Resolution()
                        {
                            Width = mode.dmPelsWidth,
                            Height = mode.dmPelsHeight
                        };
                        RefreshRate refreshRate = new RefreshRate(mode.dmDisplayFrequency);

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
            }
            
            // Enumerating devices twice because we need the GUID that doesn't appear above. Devil's Details also does this...
            Direct3D d3d = new Direct3D();
            foreach (AdapterInformation adapter in d3d.Adapters)
            {
                GraphicsAdapter toComplete = toReturn.Find(elem => adapter.Details.DeviceName.Equals(elem.Name)
                                                        && adapter.Details.Description.Equals(elem.Description));
                if (toComplete != null)
                {
                    toComplete.GUID = adapter.Details.DeviceIdentifier.ToString();
                    toComplete.Index = adapter.Adapter;
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
