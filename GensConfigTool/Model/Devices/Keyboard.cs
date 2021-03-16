using ConfigurationTool.Model.Input;
using SharpDX.DirectInput;
using System.Collections.Generic;

namespace ConfigurationTool.Model.Devices
{
    class Keyboard : InputDevice
    {
        public override bool IsConnected => true;

        readonly SharpDX.DirectInput.Keyboard keyboard = new SharpDX.DirectInput.Keyboard(new DirectInput());

        protected override int[] InvalidKeyCodes => new int[] {
            (int)Key.LeftWindowsKey,
            (int)Key.RightWindowsKey
        };

        public Keyboard()
        {
            this.Name = "Keyboard";
            this.GUID = "00000001-0000-0000-0000-000000000000";
            this.DeviceType = DeviceType.KEYBOARD;
        }

        public static Keyboard DeSerialize(string serialized)
        {
            Keyboard toReturn = new Keyboard();

            string[] split = serialized.Split('$');
            for (int i = 1; i < split.Length - 1; ++i)
            {
                split[i] = split[i].Substring(2);
            }

            toReturn.GUID = split[1];
            toReturn.Buttons = ButtonConfiguration.DeSerialize(split[2]);
            toReturn.AxisMap = new AxisMap(); // It's all unknown so we force the default
            toReturn.Deadzone = int.Parse(split[4]);
            return toReturn;
        }

        protected override int GetCurrentKey()
        {
            keyboard.Acquire();
            List<Key> keys = keyboard.GetCurrentState().PressedKeys;
            return keys.Count > 0 ? (int)keys[0] : 0;
        }
    }
}
