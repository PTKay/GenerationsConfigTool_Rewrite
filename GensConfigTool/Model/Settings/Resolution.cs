using System;
using System.Diagnostics.CodeAnalysis;

namespace ConfigurationTool.Settings.Model
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

        public override bool Equals(object obj)
        {
            Resolution res = (Resolution)obj;

            return Width == res.Width && Height == res.Height && Frequency == res.Frequency;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Width, Height, Frequency);
        }
    }
}
