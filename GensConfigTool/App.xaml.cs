using ConfigurationTool.Handlers;
using ConfigurationTool.Helpers;
using System;
using System.Globalization;
using System.Windows;
using static ConfigurationTool.Helpers.ThemeHelper;

namespace ConfigurationTool
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("en-US");
            string[] args = Environment.GetCommandLineArgs();
            if (args.Length > 1)
            {
                RegistryHandler.FixRegistry(int.Parse(args[1]));
                Application.Current.Shutdown();
            }
            NvidiaHandler.InitializeDedicatedGraphics();
        }
    }
}
