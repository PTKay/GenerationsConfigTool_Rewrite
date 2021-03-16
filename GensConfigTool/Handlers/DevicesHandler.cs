using ConfigurationTool.Model.Devices;
using ConfigurationTool.Settings.Model;
using SharpDX.Direct3D9;
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
            // Still haven't found a way to poll audio devices so this will have to do
            yield return new AudioDevice();
            yield return new AudioDevice()
            {
                Name = "None",
                GUID = "00000000-0000-0000-0000-000000000000"
            };
        }
    }
}
