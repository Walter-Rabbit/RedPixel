using RedPixel.Core.Colors.ValueObjects;
using RedPixel.Core.Models;
using Color = System.Drawing.Color;

namespace RedPixel.Core.Colors;

using SystemColor = Color;

public class RgbColorSpace : IColorSpace
{
    public static ValueObjects.Color ToRgb(in ValueObjects.Color color,
        ColorComponents components = ColorComponents.All)
    {
        if (components == ColorComponents.All)
            return color;

        var fc = (components & ColorComponents.First) != 0 ? color.FirstComponent : 0;
        var sc = (components & ColorComponents.Second) != 0 ? color.SecondComponent : 0;
        var tc = (components & ColorComponents.Third) != 0 ? color.ThirdComponent : 0;

        return new ValueObjects.Color(fc, sc, tc);
    }

    public static ValueObjects.Color FromRgb(in ValueObjects.Color color)
    {
        return color;
    }

    public static void ToRgb(Bitmap bitmap, ColorComponents components = ColorComponents.All)
    {
        if (components == ColorComponents.All)
            return;

        for (var x = 0; x < bitmap.Width; x++)
        for (var y = 0; y < bitmap.Height; y++)
            bitmap.Matrix[y, x] = ToRgb(in bitmap.Matrix[y, x], components);
    }

    public static void FromRgb(Bitmap bitmap)
    {
    }
}