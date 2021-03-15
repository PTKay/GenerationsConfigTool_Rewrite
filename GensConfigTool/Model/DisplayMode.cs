using System.Collections.Generic;
using System.Windows;

namespace ConfigurationTool.Model
{
    class DisplayMode
    {
        public readonly bool isLetterbox;

        public static DisplayMode Letterbox = new DisplayMode(true);
        public static DisplayMode Widescreen = new DisplayMode(false);

        private DisplayMode(bool isLetterbox)
        {
            this.isLetterbox = isLetterbox;
        }

        public static IEnumerable<DisplayMode> GetAll()
        {
            yield return Letterbox;
            yield return Widescreen;
        }

        public override string ToString() => isLetterbox ?
            Application.Current.TryFindResource("Letterbox").ToString() :
            Application.Current.TryFindResource("Widescreen").ToString();
    }
}
