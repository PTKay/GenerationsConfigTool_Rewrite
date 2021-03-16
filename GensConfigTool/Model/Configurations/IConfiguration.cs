namespace ConfigurationTool.Model.Configurations
{
    // Should be able to save and load configurations from a set place
    interface IConfiguration
    {
        string ConfigFile { get; }
        public Configuration LoadConfiguration(Configuration config);
        public void SaveConfiguration(Configuration config);
    }
}
