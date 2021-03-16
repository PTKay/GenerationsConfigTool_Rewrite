using ConfigurationTool.Model.Input;

namespace ConfigurationTool.Model.Devices
{
    public enum DeviceType
    {
        XINPUT, KEYBOARD
    }

    abstract class InputDevice
    {
        public abstract DeviceType DeviceType { get; }
        public abstract bool IsConnected { get; }

        public abstract string Name { get; }
        public abstract string GUID { get; set; }

        public ButtonConfiguration Buttons = new ButtonConfiguration();

        // Apparently unused outside DInput devices
        public AxisMap AxisMap = new AxisMap();

        public int Deadzone = 0;

        public string Serialize()
        {
            return $"{Name}\n$G:{GUID}$B:{Buttons}$A:{AxisMap}$D:{Deadzone}$";
        }

        public override string ToString()
        {
            return Name;
        }

        public override bool Equals(object obj)
        {
            InputDevice device = (InputDevice)obj;

            return GUID.Equals(device.GUID);
        }

        public abstract int GetKey();
    }
}
