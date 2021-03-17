namespace ConfigurationTool.Model.Configurations
{
    // Should be able to save and load configurations from a set place
    interface IConfiguration
    {
        Configuration LoadConfiguration(Configuration config);
        void SaveConfiguration(Configuration config);
    }
}
