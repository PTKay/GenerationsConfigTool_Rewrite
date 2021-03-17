using System;
using System.Collections.Generic;
using System.Text;

namespace ConfigurationTool.Settings.Model
{
    public class DepthFormat
    {
        public readonly string Value;

        public static DepthFormat INTZ = new DepthFormat("INTZ");
        public static DepthFormat DF24 = new DepthFormat("DF24");
        public static DepthFormat DF16 = new DepthFormat("DF16");

        private DepthFormat(string value)
        {
            this.Value = value;
        }

        public static IEnumerable<DepthFormat> GetAll()
        {
            yield return INTZ;
            yield return DF24;
            yield return DF16;
        }

        public uint GetFourCC()
        {
            //return (uint)((Value[3] << 24) + (Value[2] << 16) + (Value[1] << 8) + Value[0]);
            return BitConverter.ToUInt32(Encoding.ASCII.GetBytes(Value), 0);
        }

        public static DepthFormat FromFourCC(int fourcc)
        {
            string depth = Encoding.ASCII.GetString(BitConverter.GetBytes(fourcc));
            return new DepthFormat(depth);
        }

        public override string ToString()
        {
            return Value;
        }

        public override bool Equals(object obj)
        {
            DepthFormat df = (DepthFormat)obj;
            return Value.Equals(df.Value);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }
    }
}
