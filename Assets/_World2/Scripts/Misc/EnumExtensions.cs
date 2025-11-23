using System;
using System.Collections.Generic;

namespace Stellarfarer
{
    public static class EnumExtensions
    {
        public static IEnumerable<T> GetFlags<T>(this T input)
            where T : Enum
        {
            foreach (T value in Enum.GetValues(typeof(T)))
            {
                if (Convert.ToInt32(value) != 0 && input.HasFlag(value))
                    yield return value;
            }
        }

        public static bool IsSingleFlag<T>(this T value)
            where T : Enum
        {
            int intValue = Convert.ToInt32(value);
            // true if only one bit is set (and not zero)
            return intValue != 0 && (intValue & (intValue - 1)) == 0;
        }
    }
}