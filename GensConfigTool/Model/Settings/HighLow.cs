using System.Collections.Generic;
using System.Windows;

namespace ConfigurationTool.Settings.Model
{
    class HighLow
    {
        public readonly bool isHigh;

        public static HighLow High = new HighLow(true);
        public static HighLow Low = new HighLow(false);

        private HighLow(bool isHigh)
        {
            this.isHigh = isHigh;
        }

        public static IEnumerable<HighLow> GetAll()
        {
            yield return High;
            yield return Low;
        }

        public override string ToString() => isHigh ?
            Application.Current.TryFindResource("High").ToString() :
            Application.Current.TryFindResource("Low").ToString();
    }
}
