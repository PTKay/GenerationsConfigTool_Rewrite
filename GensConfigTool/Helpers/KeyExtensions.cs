using SharpDX.DirectInput;
using System.Windows;

namespace ConfigurationTool.Helpers
{
    public static class KeyExtensions
    {
        public static string GetStringValue(this Key value)
        {
            switch((int)value)
            {
                case -1:
                    return Application.Current.TryFindResource("Null").ToString();
                case 0:
                    return Application.Current.TryFindResource("Undefined").ToString();
                default:
                    return value.ToString();
            }
        }
    }
}
