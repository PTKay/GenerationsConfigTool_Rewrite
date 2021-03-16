using SharpDX.DirectInput;
using System.Windows;

namespace ConfigurationTool.Helpers
{
    public static class KeyExtensions
    {
        public static string GetStringValue(this Key value)
        {
            return (int)value == -1 ?
                Application.Current.TryFindResource("Null").ToString() :
                value.ToString();
        }
    }
}
