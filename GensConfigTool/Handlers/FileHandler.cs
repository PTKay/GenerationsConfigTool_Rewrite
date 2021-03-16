using ConfigurationTool.Model;
using ConfigurationTool.Model.Configurations;
using System;

namespace ConfigurationTool.Handlers
{
    class FileHandler
    {
        private readonly IConfiguration RegistryConfiguration;
        private readonly IConfiguration GraphicsConfiguration;
        private readonly IConfiguration AudioConfiguration;
        private readonly IConfiguration AnalyticsConfiguration;
        private readonly IConfiguration InputConfiguration;

        public FileHandler()
        {
            RegistryConfiguration = new RegistryConfiguration();
            GraphicsConfiguration = new GraphicsConfiguration();
            AudioConfiguration = new AudioConfiguration();
            AnalyticsConfiguration = new AnalyticsConfiguration();
            InputConfiguration = new InputConfiguration();
        }

        public Configuration LoadConfiguration()
        {
            Configuration config = new Configuration();

            RegistryConfiguration.LoadConfiguration(config);
            GraphicsConfiguration.LoadConfiguration(config);
            AudioConfiguration.LoadConfiguration(config);
            AnalyticsConfiguration.LoadConfiguration(config);
            InputConfiguration.LoadConfiguration(config);

            return config;
        }

        public bool SaveConfiguration(Configuration config)
        {
            try
            {
                RegistryConfiguration.SaveConfiguration(config);
                GraphicsConfiguration.SaveConfiguration(config);
                AudioConfiguration.SaveConfiguration(config);
                AnalyticsConfiguration.SaveConfiguration(config);
                InputConfiguration.SaveConfiguration(config);
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }
    }
}
