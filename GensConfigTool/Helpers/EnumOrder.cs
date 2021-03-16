using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace ConfigurationTool.Helpers
{
    public class EnumOrder<TEnum> where TEnum : struct
    {
        public static readonly TEnum[] Values;

        static EnumOrder()
        {
            var fields = typeof(TEnum).GetFields(BindingFlags.Static | BindingFlags.Public);
            Values = Array.ConvertAll(fields, x => (TEnum)x.GetValue(null));
        }

        public static int IndexOf(TEnum value)
        {
            return Array.IndexOf(Values, value);
        }
    }
}
