using Elements.Geometry;

namespace Elements
{
    public static class ColorHelper
    {
        public static string ToHex(this Color color)
        {
            return $"#{((int)(color.Red * 255)):X2}{((int)(color.Green * 255)):X2}{((int)(color.Blue * 255)):X2}";
        }
    }
}