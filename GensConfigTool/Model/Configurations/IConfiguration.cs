namespace ConfigurationTool.Model.Configurations
{
    // Should be able to save and load configurations from a set place
    interface IConfiguration
    {
        string ConfigFile { get; }
        public BasicConfiguration LoadConfiguration(BasicConfiguration config);
        public void SaveConfiguration(BasicConfiguration config);
    }
}
