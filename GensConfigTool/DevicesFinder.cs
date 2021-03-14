using GensConfigTool.Helpers;
using GensConfigTool.Model;
using SharpDX.Direct3D9;
using System.Collections.Generic;
using System.Linq;

namespace GensConfigTool
{
    class DevicesFinder
    {
        public List<GraphicsAdapter> GetGraphicsAdapters()
        {
            List<GraphicsAdapter> toReturn = new List<GraphicsAdapter>();

            /*
            ManagementObjectSearcher searcher =
                new ManagementObjectSearcher("SELECT * FROM Win32_VideoController");
            foreach (ManagementObject mo in searcher.Get())
            {
                PropertyData currentBitsPerPixel = mo.Properties["CurrentBitsPerPixel"];
                PropertyData description = mo.Properties["Description"];
                if (currentBitsPerPixel != null && description != null)
                {
                    if (currentBitsPerPixel.Value != null)
                    {

                    }
                }
            }
            */
            /*
            GraphicsHelper.GetGraphicsAdapters().ForEach(gpu =>
            {
                GraphicsAdapter currAdapter = new GraphicsAdapter()
                {
                    Description = gpu.DeviceString,
                    Name = gpu.DeviceName,
                    GUID = null,
                    DephtFormat = null
                };

                toReturn.Add(currAdapter);

                GraphicsHelper.GetDeviceModes(currAdapter.Name).ForEach(mode =>
                {
                    currAdapter.Resolutions.Add(new Resolution()
                    {
                        Width = mode.dmPelsWidth.ToString(),
                        Height = mode.dmPelsHeight.ToString(),
                        Frequency = mode.dmDisplayFrequency.ToString()
                    });

                    //MessageBox.Show($"{mode.dmDisplayFlags}\n{mode.dmBitsPerPel}\n{mode.dmDeviceName}\n{mode.dmPelsWidth}x{mode.dmPelsHeight} ({mode.dmDisplayFrequency}hz)");
                });
            });
            */

            Direct3D d3d = new Direct3D();

            for (int i = 0; i < d3d.AdapterCount; i++)
            {
                AdapterInformation adapter = d3d.Adapters[i];
                GraphicsAdapter currAdapter = new GraphicsAdapter()
                {
                    Description = adapter.Details.Description,
                    Name = adapter.Details.DeviceName, // Maybe wrong if it has various outputs
                    GUID = adapter.Details.DeviceIdentifier.ToString(),
                    Index = i + 1
                };
                toReturn.Add(currAdapter);

                HashSet<Resolution> resolutions = new HashSet<Resolution>();
                resolutions.Add(new Resolution()
                {
                    Width = adapter.CurrentDisplayMode.Width,
                    Height = adapter.CurrentDisplayMode.Height,
                    Frequency = adapter.CurrentDisplayMode.RefreshRate
                });

                foreach (DisplayMode mode in adapter.GetDisplayModes(adapter.CurrentDisplayMode.Format))
                {
                    resolutions.Add(new Resolution()
                    {
                        Width = mode.Width,
                        Height = mode.Height,
                        Frequency = mode.RefreshRate
                    });
                }
                currAdapter.Resolutions = resolutions.ToList();

                currAdapter.Resolutions.Sort((a, b) => b.CompareTo(a));
            }

            foreach (AdapterInformation adapter in d3d.Adapters)
            {
                
            }

            return toReturn;
        }

        public IEnumerable<AudioDevice> GetAudioDevices()
        {
            yield return new AudioDevice()
            {
                Name = "Default",
                GUID = "ffffffff-ffff-ffff-ffff-ffffffffffff"
            };
            yield return new AudioDevice()
            {
                Name = "None",
                GUID = "00000000-0000-0000-0000-000000000000"
            };

        }
    }
}
