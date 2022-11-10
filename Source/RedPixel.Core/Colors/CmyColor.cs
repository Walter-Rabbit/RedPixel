using RedPixel.Core.Colors.ValueObjects;
using RedPixel.Core.Models;

namespace RedPixel.Core.Colors;

public class CmyColor : IColorSpace
{
    public static void ToRgb(ref Color color, ColorComponents components = ColorComponents.All)
    {
        var r = (components & ColorComponents.First) != 0 ? 255 - color.FirstComponent : 0;

        var g = (components & ColorComponents.Second) != 0 ? 255 - color.SecondComponent : 0;

        var b = (components & ColorComponents.Third) != 0 ? 255 - color.ThirdComponent : 0;

        color.FirstComponent = r;
        color.SecondComponent = g;
        color.ThirdComponent = b;
    }

    public static void FromRgb(ref Color rgb)
    {
        var c = 255 - rgb.FirstComponent;
        var m = 255 - rgb.SecondComponent;
        var y = 255 - rgb.ThirdComponent;

        rgb.FirstComponent = c;
        rgb.SecondComponent = m;
        rgb.ThirdComponent = y;
    }

    public static void ToRgb(Bitmap bitmap, ColorComponents components = ColorComponents.All)
    {
        for (int y = 0; y < bitmap.Height; y++)
        {
            for (int x = 0; x < bitmap.Width; x++)
            {
                ToRgb(ref bitmap.Matrix[y, x], components);
            }
        }
    }

    public static void FromRgb(Bitmap bitmap)
    {
        for (int y = 0; y < bitmap.Height; y++)
        {
            for (int x = 0; x < bitmap.Width; x++)
            {
                FromRgb(ref bitmap.Matrix[y, x]);
            }
        }
    }
}