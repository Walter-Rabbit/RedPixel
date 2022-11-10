using RedPixel.Core.Colors.ValueObjects;
using RedPixel.Core.Models;

namespace RedPixel.Core.Colors;

using SystemColor = System.Drawing.Color;

public class RgbColor : IColorSpace
{
    public static void ToRgb(ref Color color, ColorComponents components = ColorComponents.All)
    {
        if (components == ColorComponents.All)
            return;

        color.FirstComponent = (components & ColorComponents.First) != 0 ? color.FirstComponent : 0;
        color.SecondComponent = (components & ColorComponents.Second) != 0 ? color.SecondComponent : 0;
        color.ThirdComponent = (components & ColorComponents.Third) != 0 ? color.ThirdComponent : 0;
    }

    public static void FromRgb(ref Color color)
    {
    }

    public static void ToRgb(Bitmap bitmap, ColorComponents components = ColorComponents.All)
    {
        if (components == ColorComponents.All)
            return;

        for (int x = 0; x < bitmap.Width; x++)
        {
            for (int y = 0; y < bitmap.Height; y++)
            {
                ToRgb(ref bitmap.Matrix[y, x], components);
            }
        }
    }

    public static void FromRgb(Bitmap bitmap)
    {
    }
}