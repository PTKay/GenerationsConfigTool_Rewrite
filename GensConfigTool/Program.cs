using ConfigurationTool.Handlers;
using System;

namespace ConfigurationTool
{
    class Program
    {
        [STAThread]
        private static void Main(string[] args)
        {
            if (args.Length > 0)
            {
                RegistryHandler reg = new RegistryHandler();
                reg.FixRegistry(int.Parse(args[0]));
                return;
            }
            App.Main();
        }
    }
}
