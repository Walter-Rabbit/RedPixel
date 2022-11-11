using RedPixel.Core.Colors.ValueObjects;
using RedPixel.Core.Models;

namespace RedPixel.Core.Colors;

public class YCbCr709ColorSpace : IColorSpace
{
    public static Color ToRgb(in Color color, ColorComponents components = ColorComponents.All)
    {
        var y = (components & ColorComponents.First) != 0 ? color.FirstComponent : 0;
        var cb = (components & ColorComponents.Second) != 0 ? color.SecondComponent : 128;
        var cr = (components & ColorComponents.Third) != 0 ? color.ThirdComponent : 128;

        return new Color(
            y + 1.402f * (cr - 128f),
            y - 0.34414f * (cb - 128f) - 0.71414f * (cr - 128f),
            y + 1.772f * (cb - 128f)
        );
    }

    public static Color FromRgb(in Color color)
    {
        var R = color.FirstComponent;
        var G = color.SecondComponent;
        var B = color.ThirdComponent;


        var y = 0.299f * R + 0.587f * G + 0.114f * B;
        var cb = 128f - 0.168736f * R - 0.331264f * G + 0.5f * B;
        var cr = 128f + 0.5f * R - 0.418688f * G - 0.081312f * B;


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