using RedPixel.Core.Colors.ValueObjects;
using RedPixel.Core.Models;

namespace RedPixel.Core.Colors;

public class YCoCgColorSpace : IColorSpace
{
    public static Color ToRgb(in Color color, ColorComponents components = ColorComponents.All)
    {
        var luma = (components & ColorComponents.First) != 0 ? color.FirstComponent : 0;
        var cOrange = (components & ColorComponents.Second) != 0 ? color.SecondComponent : 255;
        var cGreen = (components & ColorComponents.Third) != 0 ? color.ThirdComponent : 255;

        var y = luma;
        var cO = cOrange - 255;
        var cG = cGreen - 255;

        var r = (y + cO - cG) / 2;
        var g = (y + cG) / 2;
        var b = (y - cO - cG) / 2;

        return new Color(r, g, b);
    }

    public static Color FromRgb(in Color color)
    {
        var r = color.FirstComponent * 2;
        var g = color.SecondComponent * 2;
        var b = color.ThirdComponent * 2;

        var y = (b + 2 * g + r) / 4;
        var cG = (-b + 2 * g - r) / 4 + 255;
        var cO = (-b + r) / 2 + 255;

        return new Color(y, cO, cG);
    }

    public static void ToRgb(Bitmap bitmap, ColorComponents components = ColorComponents.All)
    {
        bitmap.BytesForColor -= 1;

        for (var y = 0; y < bitmap.Height; y++)
        {
            for (var x = 0; x < bitmap.Width; x++)
            {
                bitmap.Matrix[y, x] = ToRgb(in bitmap.Matrix[y, x], components);
            }
        }
    }

    public static void FromRgb(Bitmap bitmap)
    {
        bitmap.BytesForColor += 1;

        for (var y = 0; y < bitmap.Height; y++)
        {
            for (var x = 0; x < bitmap.Width; x++)
            {
                bitmap.Matrix[y, x] = FromRgb(in bitmap.Matrix[y, x]);
            }
        }
    }
}