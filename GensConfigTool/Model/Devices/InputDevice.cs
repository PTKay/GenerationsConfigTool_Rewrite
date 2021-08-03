using ConfigurationTool.Model.Input;
using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace ConfigurationTool.Model.Devices
{
    public enum DeviceType { XINPUT, DINPUT, KEYBOARD }
    public enum MovementType { DPAD, JOYSTICK, KEYBOARD }

    abstract class InputDevice
    {
        public DeviceType DeviceType;
        public abstract bool IsConnected { get; }

        public string Name;
        public string GUID;
        public MovementType MovementType;

        public ButtonConfiguration Buttons = new ButtonConfiguration();
        public AxisMap AxisMap = new AxisMap(); // Only used when MovementType is set to JOYSTICK
        public int Deadzone = 0;

        protected abstract int[] InvalidKeyCodes { get; }
        protected abstract int GetCurrentKey();

        public string Serialize()
        {
            return $"{Name}\n$G:{GUID}$B:{Buttons} {(int)MovementType}$A:{AxisMap}$D:{Deadzone}$";
        }

        public async Task SetKey(string keyName, InputDevice targetDevice, Action<int> keyConsumer)
        {
            int key = -1;

            await Task.Run(() =>
            {
                SpinWait.SpinUntil(() =>
                {
                    key = GetCurrentKey();
                    return key != 0 && !Array.Exists(InvalidKeyCodes, elem => elem == key);
                });
            });

            if (key != -1)
            {
                FieldInfo targetProperty = null;
                FieldInfo[] fields = typeof(ButtonConfiguration).GetFields();

                for (int i = 0; i < fields.Length; ++i)
                {
                    FieldInfo currField = fields[i];
                    if (currField.Name.Equals(keyName))
                    {
                        targetProperty = currField;
                    }
                    if ((int)currField.GetValue(targetDevice.Buttons) == key)
                    {
                        currField.SetValue(targetDevice.Buttons, 0);
                    }

                }

                // Change when implementing Dinput
                targetProperty?.SetValue(targetDevice.Buttons, key);
            }

            keyConsumer(key);
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

        public override int GetHashCode()
        {
            return GUID.GetHashCode();
        }
    }
}
