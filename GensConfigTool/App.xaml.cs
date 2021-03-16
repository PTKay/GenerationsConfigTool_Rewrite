using ConfigurationTool.Handlers;
using System;
using System.Windows;

namespace ConfigurationTool
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            string[] args = Environment.GetCommandLineArgs();
            if (args.Length > 1)
            {
                MessageBox.Show(args[1]);
                RegistryHandler.FixRegistry(int.Parse(args[1]));
                Application.Current.Shutdown();
            }
            NvidiaHandler.InitializeDedicatedGraphics();
        }
    }
}
