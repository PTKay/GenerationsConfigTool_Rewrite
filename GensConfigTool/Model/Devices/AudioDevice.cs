namespace ConfigurationTool.Model.Devices
{
    public class AudioDevice
    {
        public string Name { get; set; }
        public string GUID { get; set; }

        public override string ToString() => Name;

        public AudioDevice()
        {
            // Start with Default
            this.Name = "Default";
            this.GUID = "ffffffff-ffff-ffff-ffff-ffffffffffff";
        }

        public override bool Equals(object obj)
        {
            AudioDevice dev = (AudioDevice)obj;
            return GUID.Equals(dev.GUID);
        }
    }
}
