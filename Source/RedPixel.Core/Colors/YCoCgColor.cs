using RedPixel.Core.Colors.ValueObjects;
using RedPixel.Core.Models;

namespace RedPixel.Core.Colors;

public class YCoCgColor : IColorSpace
{
    public static void ToRgb(ref Color color, ColorComponents components = ColorComponents.All)
    {
        var luma = (components & ColorComponents.First) != 0 ? color.FirstComponent : 0;
        var cOrange = (components & ColorComponents.Second) != 0 ? color.SecondComponent : 0;
        var cGreen = (components & ColorComponents.Third) != 0 ? color.ThirdComponent : 0;

        var y = luma;
        var cO = cOrange - 255;
        var cG = cGreen - 255;

        color.FirstComponent = (y + cO - cG)/2;
        color.SecondComponent = (y + cG)/2;
        color.ThirdComponent = (y - cO - cG)/2;
    }

    public static void FromRgb(ref Color color)
    {
        var r = color.FirstComponent * 2;
        var g = color.SecondComponent * 2;
        var b = color.ThirdComponent * 2;

        color.FirstComponent = (b + 2*g + r)/4;
        color.SecondComponent = (-b + r) / 2 + 255;
        color.ThirdComponent = (-b + 2*g - r)/4 + 255;
    }

    public static void ToRgb(Bitmap bitmap, ColorComponents components = ColorComponents.All)
    {
        bitmap.BytesForColor -= 1;

        for (var y = 0; y < bitmap.Height; y++)
        {
            for (var x = 0; x < bitmap.Width; x++)
            {
                ToRgb(ref bitmap.Matrix[y, x], components);
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
                FromRgb(ref bitmap.Matrix[y, x]);
            }
        }
    }
}