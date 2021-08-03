using SharpDX.XInput;
using System.Reflection;
using System.Windows;

namespace ConfigurationTool.Model.Devices
{
    class XinputController : InputDevice
    {
        public override bool IsConnected => Controller.IsConnected;
        protected override int[] InvalidKeyCodes => new int[] { };

        private readonly Controller Controller;

        public XinputController(UserIndex port)
        {
            this.Name = Application.Current.TryFindResource("XinputDevice").ToString();
            this.GUID = "00000000-0000-0000-0000-000000000000";
            this.DeviceType = DeviceType.XINPUT;
            this.MovementType = MovementType.JOYSTICK; // Unused
            this.Controller = new Controller(port);

            foreach (FieldInfo field in Buttons.GetType().GetFields())
            {
                field.SetValue(Buttons, -1);
            }
        }

        protected override int GetCurrentKey() => -1;
    }
}
