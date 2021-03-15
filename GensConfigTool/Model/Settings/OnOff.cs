using System.Collections.Generic;
using System.Windows;

namespace ConfigurationTool.Settings.Model
{
    class OnOff
    {
        public readonly bool isOn;

        public static OnOff On = new OnOff(true);
        public static OnOff Off = new OnOff(false);

        private OnOff(bool isOn)
        {
            this.isOn = isOn;
        }

        public static IEnumerable<OnOff> GetAll()
        {
            yield return On;
            yield return Off;
        }

        public override string ToString() => isOn ?
            Application.Current.TryFindResource("On").ToString() :
            Application.Current.TryFindResource("Off").ToString();
    }
}
