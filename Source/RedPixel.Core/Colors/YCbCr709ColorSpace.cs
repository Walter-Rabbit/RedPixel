using RedPixel.Core.Colors.ValueObjects;
using RedPixel.Core.Models;

namespace RedPixel.Core.Colors;

public class YCbCr709ColorSpace : IColorSpace
{
    public static Color ToRgb(in Color color, ColorComponents components = ColorComponents.All)
    {
        var y = (components & ColorComponents.First) != 0 ? color.FirstComponent : 0;
        var cb = (components & ColorComponents.Second) != 0 ? color.SecondComponent - 128 : 0;
        var cr = (components & ColorComponents.Third) != 0 ? color.ThirdComponent - 128 : 0;

        var a = 0.2126f;
        var b = 0.7152f;
        var c = 0.0722f;

        var d = 1.8556f;
        var e = 1.5748f;

        cb *= d;
        cr *= e;

        var B = cb + y;
        var R = cr + y;
        var G = (y - a * R - c * B) / b;
        
        return new Color(
            Math.Max(0, Math.Min(255, R)),
            Math.Max(0, Math.Min(255, G)),
            Math.Max(0, Math.Min(255, B))
        );
    }

    public static Color FromRgb(in Color color)
    {
        var R = color.FirstComponent;
        var G = color.SecondComponent;
        var B = color.ThirdComponent;


        var y = 0.2126f * R + 0.7152f * G + 0.0722f * B;
        var cb = -0.1146f * R - 0.3854f * G + 0.5f * B + 128;
        var cr = 0.5f * R - 0.4542f * G - 0.0458f * B + 128;


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