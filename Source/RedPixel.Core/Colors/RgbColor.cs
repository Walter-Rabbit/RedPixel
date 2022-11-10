using RedPixel.Core.Colors.ValueObjects;
using RedPixel.Core.Models;

namespace RedPixel.Core.Colors;

using SystemColor = System.Drawing.Color;

public class RgbColor : IColorSpace
{
    public static Color ToRgb(in Color color, ColorComponents components = ColorComponents.All)
    {
        if (components == ColorComponents.All)
            return color;

        var fc = (components & ColorComponents.First) != 0 ? color.FirstComponent : 0;
        var sc = (components & ColorComponents.Second) != 0 ? color.SecondComponent : 0;
        var tc = (components & ColorComponents.Third) != 0 ? color.ThirdComponent : 0;

        return new Color(fc, sc, tc);
    }

    public static Color FromRgb(in Color color)
    {
        return color;
    }

    public static void ToRgb(Bitmap bitmap, ColorComponents components = ColorComponents.All)
    {
        if (components == ColorComponents.All)
            return;

        for (int x = 0; x < bitmap.Width; x++)
        {
            for (int y = 0; y < bitmap.Height; y++)
            {
                bitmap.Matrix[y, x] = ToRgb(in bitmap.Matrix[y, x], components);
            }
        }
    }

    public static void FromRgb(Bitmap bitmap)
    {
    }
}