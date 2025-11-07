namespace Stellarfarer
{
    public static class EnumExtensions
    {
        public static System.Collections.Generic.IEnumerable<T> GetFlags<T>(this T input)
            where T : System.Enum
        {
            foreach (T value in System.Enum.GetValues(typeof(T)))
            {
                if (System.Convert.ToInt32(value) != 0 && input.HasFlag(value))
                    yield return value;
            }
        }

        public static bool IsSingleFlag<T>(this T value)
            where T : System.Enum
        {
            int intValue = System.Convert.ToInt32(value);
            // true if only one bit is set (and not zero)
            return intValue != 0 && (intValue & (intValue - 1)) == 0;
        }
    }
}