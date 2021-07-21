using ConfigurationTool.Model.Settings;
using SharpDX.DirectInput;
using System;
using System.Collections.Generic;
using System.Windows;

namespace ConfigurationTool.Helpers
{
    public static class Extensions
    {
        // Key extensions
        public static string GetStringValue(this Key value)
        {
            switch ((int)value)
            {
                case -1:
                    return Application.Current.TryFindResource("Null").ToString();
                case 0:
                    return Application.Current.TryFindResource("Undefined").ToString();
                default:
                    return value.ToString();
            }
        }

        public static void InsertElementDescending<T>(this List<T> source,
        T toAdd) where T : IComparable
        {
            int index = source.FindLastIndex(elem => elem.CompareTo(toAdd) > 0);
            source.Insert(index + 1, toAdd);
        }

        public static void InsertElementAscending<T>(this List<T> source,
T toAdd) where T : IComparable
        {
            int index = source.FindIndex(elem => elem.CompareTo(toAdd) < 0);
            source.Insert(index + 1, toAdd);
        }
    }
}
