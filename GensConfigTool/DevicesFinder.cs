using ConfigurationTool.Model;
using SharpDX.Direct3D9;
using System.Collections.Generic;

namespace ConfigurationTool
{
    class DevicesFinder
    {
        public List<GraphicsAdapter> GetGraphicsAdapters()
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

                /*
                // Adding the current resolution to avoid it not being detected in GetDisplayModes (yes, that happens).
                // Also using HashSet just in case it appears again.
                HashSet<Resolution> resolutions = new HashSet<Resolution>();

                resolutions.Add(new Resolution()
                {
                    Width = adapter.CurrentDisplayMode.Width,
                    Height = adapter.CurrentDisplayMode.Height,
                    Frequency = adapter.CurrentDisplayMode.RefreshRate
                });
                */

                foreach (SharpDX.Direct3D9.DisplayMode mode in adapter.GetDisplayModes(adapter.CurrentDisplayMode.Format))
                {
                    currAdapter.Resolutions.Add(new Resolution()
                    {
                        Width = mode.Width,
                        Height = mode.Height,
                        Frequency = mode.RefreshRate
                    });
                }

                //currAdapter.Resolutions = resolutions.ToList();

                currAdapter.Resolutions.Sort((a, b) => b.CompareTo(a));
            }

            return toReturn;
        }

        public IEnumerable<AudioDevice> GetAudioDevices()
        {
            // Still haven't found a way to poll audio devices so this will have to do

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
