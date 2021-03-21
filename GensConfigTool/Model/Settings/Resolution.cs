using ConfigurationTool.Model.Settings;
using System;
using System.Collections.Generic;

namespace ConfigurationTool.Settings.Model
{
    public class Resolution : IComparable
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public List<RefreshRate> RefreshRates = new List<RefreshRate>();

        public int CompareTo(object other)
        {
            Resolution res = (Resolution)other;

            if (other == null)
            {
                return 1;
            }

            if (this.Width == res.Width)
            {
                return this.Height.CompareTo(res.Height);
            }

            return this.Width.CompareTo(res.Width);
        }
        public override string ToString() => $"{Width} x {Height}";

        public override bool Equals(object obj)
        {
            Resolution res = (Resolution)obj;

            return Width == res.Width && Height == res.Height;
        }

        public override int GetHashCode()
        {
            return Tuple.Create(Width, Height).GetHashCode();
        }
    }
}
