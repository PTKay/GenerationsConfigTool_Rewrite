using ConfigurationTool.Settings.Model;
using System;
using System.Collections.Generic;

namespace ConfigurationTool.Model.Devices
{
    public class GraphicsAdapter : IComparable
    {
        public String Description { get; set; }
        public String Name { get; set; }
        public String GUID { get; set; }
        public int Index { get; set; }
        public List<Resolution> Resolutions = new List<Resolution>();

        public override String ToString() => $"{Description} (Display {Index + 1})";

        public override bool Equals(object obj)
        {
            GraphicsAdapter adapter = (GraphicsAdapter)obj;
            return Description.Equals(adapter.Description) &&
                Name.Equals(adapter.Name) &&
                GUID.Equals(adapter.GUID) &&
                Index == adapter.Index;
        }

        public override int GetHashCode()
        {
            return Tuple.Create(Description, Name, GUID, Index).GetHashCode();
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
