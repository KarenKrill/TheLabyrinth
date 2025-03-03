using UnityEngine;

namespace KarenKrill.Utilities
{
    internal static class ColorExtensions
    {
        public static int ToRgbValue(this Color color) => ((int)(color.r * 255) << 16) | ((int)(color.g * 255) << 8) | (int)(color.b * 255);
        public static int ToRgbaValue(this Color color) => (ToRgbValue(color) << 8) | (int)(color.a * 255f);
        public static int ToArgbValue(this Color color) => ((int)(color.a * 255f) << 24) | ToRgbValue(color);
        public static string ToRgbHexStr(this Color color) => color.ToRgbValue().ToString("x6");
        public static string ToRgbaHexStr(this Color color) => color.ToRgbaValue().ToString("x8");
        public static string ToArgbHexStr(this Color color) => color.ToArgbValue().ToString("x8");
    }
}