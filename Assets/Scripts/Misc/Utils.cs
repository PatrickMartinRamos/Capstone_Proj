using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace CapstoneProj.MiscSystem
{
    public static class Utils
    {
        public static string RemoveWhiteSpaceAndDash(string str)
            => Regex.Replace(str, @"[\s\-]", "");

        public static List<T> GetSelectedFlags<T>(this T selected) where T : Enum
        {
            List<T> selectedList = new List<T>();

            foreach (T type in Enum.GetValues(typeof(T)))
            {
                // Skip the zero (None) value
                if (Convert.ToInt32(type) == 0)
                    continue;

                if (selected.HasFlag(type))
                    selectedList.Add(type);
            }

            return selectedList;
        }
    }
}