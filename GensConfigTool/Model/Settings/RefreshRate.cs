using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigurationTool.Model.Settings
{
    public class RefreshRate : IComparable
    {
        public readonly int Value;

        public RefreshRate(int value)
        {
            this.Value = value;
        }

        public override string ToString()
        {
            return $"{Value}Hz";
        }

        public int CompareTo(object other)
        {
            RefreshRate refreshRate = (RefreshRate)other;

            if (refreshRate == null)
            {
                return 1;
            }

            return this.Value.CompareTo(refreshRate.Value);
        }

        public override bool Equals(object obj)
        {
            RefreshRate toCompare = (RefreshRate)obj;
            return Value == toCompare.Value;
        }
    }
}
