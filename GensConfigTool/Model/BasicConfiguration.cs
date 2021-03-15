namespace ConfigurationTool.Model
{
    class BasicConfiguration
    {
        public Resolution Resolution;
        public GraphicsAdapter GraphicsAdapter;
        public DepthFormat DepthFormat = DepthFormat.INTZ;
        public AudioDevice AudioDevice;

        public OnOff Antialiasing = OnOff.On;
        public OnOff VSync = OnOff.On;
        public OnOff Analytics = OnOff.On;

        public HighLow ShadowQuality = HighLow.High;
        public HighLow ReflectionQuality = HighLow.High;

        public DisplayMode DisplayMode = DisplayMode.Widescreen;

    }
}
