using ConfigurationTool.Model.Devices;
using ConfigurationTool.Settings.Model;

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
    }
}
