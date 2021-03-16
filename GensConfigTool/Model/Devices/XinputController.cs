using SharpDX.XInput;
using System.Windows;

namespace ConfigurationTool.Model.Devices
{
    class XinputController : InputDevice
    {
        public override bool IsConnected => Controller.IsConnected;
        public override DeviceType DeviceType => DeviceType.XINPUT;

        public override string Name => Application.Current.TryFindResource("XinputDevice").ToString();

        private string _guid = "00000000-0000-0000-0000-000000000000";
        public override string GUID { get => _guid; set => _guid = value; }

        private readonly Controller Controller;

        public XinputController(UserIndex port)
        {
            Controller = new Controller(port);
        }

        public override int GetKey()
        {
            return -1;
        }
    }
}
