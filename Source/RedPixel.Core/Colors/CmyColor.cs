using RedPixel.Core.Colors.ValueObjects;
using RedPixel.Core.Models;
using Color = RedPixel.Core.Colors.ValueObjects.Color;

namespace RedPixel.Core.Colors;

public class CmyColor : IColorSpace
{
    public static Color ToRgb(in Color color, ColorComponents components = ColorComponents.All)
    {
        var r = (components & ColorComponents.First) != 0 ? 255 - color.FirstComponent : 0;

        var g = (components & ColorComponents.Second) != 0 ? 255 - color.SecondComponent : 0;

        var b = (components & ColorComponents.Third) != 0 ? 255 - color.ThirdComponent : 0;

        return new Color(r, g, b);
    }

    public static Color FromRgb(in Color rgb)
    {
        var c = 255 - rgb.FirstComponent;
        var m = 255 - rgb.SecondComponent;
        var y = 255 - rgb.ThirdComponent;

        return new Color(c, m, y);
    }

    public static void ToRgb(Bitmap bitmap, ColorComponents components = ColorComponents.All)
    {
        for (int y = 0; y < bitmap.Height; y++)
        {
            for (int x = 0; x < bitmap.Width; x++)
            {
                bitmap.Matrix[y, x] = ToRgb(in bitmap.Matrix[y, x], components);
            }
        }
    }

    public static void FromRgb(Bitmap bitmap)
    {
        for (int y = 0; y < bitmap.Height; y++)
        {
            for (int x = 0; x < bitmap.Width; x++)
            {
                bitmap.Matrix[y, x] = FromRgb(in bitmap.Matrix[y, x]);
            }
        }
    }
}