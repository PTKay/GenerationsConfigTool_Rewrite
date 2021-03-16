using ConfigurationTool.Model;
using ConfigurationTool.Model.Configurations;

namespace ConfigurationTool.Handlers
{
    class FileHandler
    {
        private readonly IConfiguration GraphicsConfiguration;
        private readonly IConfiguration AudioConfiguration;
        private readonly IConfiguration AnalyticsConfiguration;
        private readonly IConfiguration InputConfiguration;

        public FileHandler(string inputLocation)
        {
            GraphicsConfiguration = new GraphicsConfiguration();
            AudioConfiguration = new AudioConfiguration();
            AnalyticsConfiguration = new AnalyticsConfiguration();
            InputConfiguration = new InputConfiguration(inputLocation);
        }

        public Configuration LoadConfiguration()
        {
            Configuration config = new Configuration();

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
                GraphicsConfiguration.SaveConfiguration(config);
                AudioConfiguration.SaveConfiguration(config);
                AnalyticsConfiguration.SaveConfiguration(config);
                InputConfiguration.SaveConfiguration(config);
            }
            catch
            {
                return false;
            }
            return true;
        }
    }
}
