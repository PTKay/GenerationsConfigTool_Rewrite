using ConfigurationTool.Model;
using ConfigurationTool.Model.Configurations;

namespace ConfigurationTool.Handlers
{
    class FileHandler
    {
        private static readonly IConfiguration GraphicsConfiguration = new GraphicsConfiguration();
        private static readonly IConfiguration AudioConfiguration = new AudioConfiguration();
        private static readonly IConfiguration AnalyticsConfiguration = new AnalyticsConfiguration();

        public static BasicConfiguration LoadBasicConfiguration()
        {
            BasicConfiguration config = new BasicConfiguration();

            GraphicsConfiguration.LoadConfiguration(config);
            AudioConfiguration.LoadConfiguration(config);
            AnalyticsConfiguration.LoadConfiguration(config);

            return config;
        }

        public static bool SaveBasicConfiguration(BasicConfiguration config)
        {
            try
            {
                GraphicsConfiguration.SaveConfiguration(config);
                AudioConfiguration.SaveConfiguration(config);
                AnalyticsConfiguration.SaveConfiguration(config);
            }
            catch
            {
                return false;
            }
            return true;
        }
    }
}
