namespace GensConfigTool.Model
{
    public class AudioDevice
    {
        public string Name { get; set; }
        public string GUID { get; set; }

        public override string ToString() => Name;
    }
}
