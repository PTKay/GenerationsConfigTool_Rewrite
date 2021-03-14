using System;
using System.Diagnostics.CodeAnalysis;

namespace GensConfigTool.Model
{
    public class Resolution : IComparable<Resolution>
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public int Frequency { get; set; }

        public int CompareTo([AllowNull] Resolution other)
        {
            if (other == null)
            {
                return 1;
            }

            if (this.Width == other.Width && this.Height == other.Height)
            {
                return this.Frequency.CompareTo(other.Frequency);
            }

            if (this.Width == other.Width)
            {
                return this.Height.CompareTo(other.Height);
            }

            return this.Width.CompareTo(other.Width);
        }

        public override string ToString() => $"{Width} x {Height} ({Frequency} hz)";
    }
}
