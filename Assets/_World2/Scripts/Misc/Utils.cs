using UnityEngine;

namespace Stellarfarer
{
    public static class Utils
    {
        public static int StringToInt(string str)
        {
            if (string.IsNullOrWhiteSpace(str))
                return default;

            if (int.TryParse(str, out int value))
                return value;
            else
            {
                Debug.LogError($"Cannot convert \"{str}\" to int.");
                return 0;
            }
        }

        public static float StringToFloat(string str)
        {
            if (string.IsNullOrWhiteSpace(str))
                return default;

            if (float.TryParse(str, out float value))
                return value;
            else
            {
                Debug.LogError($"Cannot convert \"{str}\" to float.");
                return 0f;
            }
        }

        public static TEnum StringToEnumFlag<TEnum>(string str)
            where TEnum : struct, System.Enum
        {
            if (string.IsNullOrWhiteSpace(str))
                return default;

            string[] parts = str.Split('|', System.StringSplitOptions.RemoveEmptyEntries);

            TEnum result = default;

            foreach (var part in parts)
            {
                if (System.Enum.TryParse(part.Trim(), true, out TEnum value))
                    result = (TEnum)(object)(((int)(object)result) | ((int)(object)value));
                else
                {
                    Debug.LogError($"Invalid enum flag value \"{part}\" for enum type {typeof(TEnum).FullName}.");
                    return default;
                }
            }

            return result;
        }

        public static Color StringToColor(string str)
        {
            if (string.IsNullOrWhiteSpace(str))
                return default;

            str = str.Trim().ToLowerInvariant();

            return str switch
            {
                "lightblue" => Color.lightBlue,
                "purple" => Color.purple,
                "green" => Color.green,
                "orange" => Color.orange,
                "gray" => Color.gray,
                "darkblue" => Color.darkBlue,
                _ => default
            };
        }

        public static Vector2 ConvertPixelsToUIUnits(Vector2 sizeInPixels, float pixelsPerUnit)
            => sizeInPixels / pixelsPerUnit;

        public static float MultiSubtract(float num1, float num2, float num3)
            => num1 - num2 - num3;

        public static float Halve(float num)
            => num / 2;

        public static Vector2 Halve(Vector2 vector)
            => vector / 2;

        public static float GetDistanceFromOriginToAxisCenter(
            Vector2 spriteBorder,
            float textureSize)
        {
            float startToAxis_X = spriteBorder.x;
            float startToAxis_Y = textureSize - spriteBorder.y;

            float halfTextureSize = Halve(textureSize);

            float axisCenter = (startToAxis_X + startToAxis_Y) / 2;
            float originToAxisCenter = axisCenter - halfTextureSize;

            return originToAxisCenter;
        }
    }
}