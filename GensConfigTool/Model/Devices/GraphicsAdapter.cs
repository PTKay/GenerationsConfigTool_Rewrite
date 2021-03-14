using System;
using System.Collections.Generic;

namespace GensConfigTool.Model
{
    public class GraphicsAdapter
    {
        public String Description { get; set; }
        public String Name { get; set; }
        public String GUID { get; set; }
        public String DepthFormat { get; set; }
        public int Index { get; set; }
        public List<Resolution> Resolutions = new List<Resolution>();

        public GraphicsAdapter()
        {
            /* INTZ (endianess is reversed)
             * Supported formats are:
             * - INTZ
             * - DF16
             * - DF24
             * Currently we're forcing INTZ since most GPUs support it and I can't find a way to get this info
             */
            this.DepthFormat = "1515474505";
        }

        public override String ToString() => $"{Description} (Display {Index})";
    }
}
