using ConfigurationTool.Model.Devices;
using ConfigurationTool.Model.Settings;
using ConfigurationTool.Settings.Model;
using System.Security.Principal;

namespace ConfigurationTool.Model
{
    class Configuration
    {
        public Resolution Resolution;
        public GraphicsAdapter GraphicsAdapter;
        public DepthFormat DepthFormat = DepthFormat.INTZ;
        public AudioDevice AudioDevice = new AudioDevice();

        public Keyboard Keyboard = new Keyboard();
        public XinputController XinputController = new XinputController(0);

        public OnOff Antialiasing = OnOff.On;
        public OnOff VSync = OnOff.On;
        public OnOff Analytics = OnOff.On;

        public HighLow ShadowQuality = HighLow.High;
        public HighLow ReflectionQuality = HighLow.High;

        public DisplayMode DisplayMode = DisplayMode.Letterbox;

        public bool ProcessIsElevated
        {
            get
            {
                return WindowsIdentity.GetCurrent().Owner
                    .IsWellKnown(WellKnownSidType.BuiltinAdministratorsSid);
            }
        }

        public Language Language = Language.English;
        // Relative to user Documents folder
        public string InputSaveLocation = "My Games\\Sonic Generations\\Saved Games";
    }
}
