using RedPixel.Core.Colors.ValueObjects;
using RedPixel.Core.Models;

namespace RedPixel.Core.Colors;

public class YCbCr601ColorSpace : IColorSpace
{
    public static Color ToRgb(in Color color, ColorComponents components = ColorComponents.All)
    {
        var y = (components & ColorComponents.First) != 0 ? color.FirstComponent : 0;
        var cb = (components & ColorComponents.Second) != 0 ? color.SecondComponent : 128;
        var cr = (components & ColorComponents.Third) != 0 ? color.ThirdComponent : 128;

        return new Color(
            298.082f / 256 * y + 408.583f / 256 * cr - 222.921f,
            296.082f / 256 * y - 100.291f / 256 * cb - 208.120f / 256 * cr + 135.576f,
            298.082f / 256 * y + 516.412f / 256 * cb - 276.836f
        );
    }

    public static Color FromRgb(in Color color)
    {
        var r = color.FirstComponent;
        var g = color.SecondComponent;
        var b = color.ThirdComponent;

        var y = 16 + (65.738f * r + 129.057f * g + 25.064f * b) / 256;
        var cb = 128 + (-37.945f * r - 74.494f * g + 112.439f * b) / 256;
        var cr = 128 + (112.439f * r - 94.154f * g - 18.285f * b) / 256;

        return new Color(y, cb, cr);
    }

    public static void ToRgb(Bitmap bitmap, ColorComponents components = ColorComponents.All)
    {
        for (int y = 0; y < bitmap.Height; y++)
        {
            for (int x = 0; x < bitmap.Width; x++)
            {
                bitmap.Matrix[y, x] = ToRgb(bitmap.Matrix[y, x], components);
            }
        }
    }

    public static void FromRgb(Bitmap bitmap)
    {
        for (int y = 0; y < bitmap.Height; y++)
        {
            for (int x = 0; x < bitmap.Width; x++)
            {
                bitmap.Matrix[y, x] = FromRgb(bitmap.Matrix[y, x]);
            }
        }
    }
}