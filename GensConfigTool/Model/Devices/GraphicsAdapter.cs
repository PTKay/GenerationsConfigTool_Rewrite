using ConfigurationTool.Settings.Model;
using System;
using System.Collections.Generic;

namespace ConfigurationTool.Model.Devices
{
    public class GraphicsAdapter : IComparable
    {
        public String Description { get; set; } // GPU Name 
        public String Name { get; set; } // Monitor index (\\.\DISPLAY1)
        public String GUID { get; set; } // GPU GUID
        public int Index { get; set; } // Monitor index
        public String MonitorID { get; set; } // Monitor ID

        public List<Resolution> Resolutions = new List<Resolution>();

        public override String ToString() => $"{Description} (Display {Index + 1})";

        public override bool Equals(object obj)
        {
            GraphicsAdapter adapter = (GraphicsAdapter)obj;
            return Description.Equals(adapter.Description) &&
                MonitorID.Equals(adapter.MonitorID) &&
                GUID.Equals(adapter.GUID);
        }

        public override int GetHashCode()
        {
            return Tuple.Create(Description, Name, GUID).GetHashCode();
        }
        public int CompareTo(object other)
        {
            GraphicsAdapter adapter = (GraphicsAdapter)other;

            if (adapter == null)
            {
                return 1;
            }

            return this.Index.CompareTo(adapter.Index);
        }
    }
}
