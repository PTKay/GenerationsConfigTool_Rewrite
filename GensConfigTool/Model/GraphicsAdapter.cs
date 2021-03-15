using ConfigurationTool.Settings.Model;
using System;
using System.Collections.Generic;

namespace ConfigurationTool.Model
{
    public class GraphicsAdapter
    {
        public String Description { get; set; }
        public String Name { get; set; }
        public String GUID { get; set; }
        public int Index { get; set; }
        public List<Resolution> Resolutions = new List<Resolution>();

        public override String ToString() => $"{Description} (Display {Index})";

        public override bool Equals(object obj)
        {
            GraphicsAdapter adapter = (GraphicsAdapter)obj;
            return Description.Equals(adapter.Description) &&
                Name.Equals(adapter.Name) &&
                GUID.Equals(adapter.GUID) &&
                Index == adapter.Index;
        }
    }
}
