namespace ConfigurationTool.Model
{
    public class AudioDevice
    {
        public string Name { get; set; }
        public string GUID { get; set; }

        public override string ToString() => Name;

        public override bool Equals(object obj)
        {
            AudioDevice dev = (AudioDevice)obj;
            return Name.Equals(dev.Name) && GUID.Equals(dev.GUID);
        }
    }
}
