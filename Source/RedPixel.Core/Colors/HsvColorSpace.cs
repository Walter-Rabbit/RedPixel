using RedPixel.Core.Colors.ValueObjects;
using RedPixel.Core.Models;

namespace RedPixel.Core.Colors;

public class HsvColorSpace : IColorSpace
{
    public static Color ToRgb(in Color color, ColorComponents components = ColorComponents.All)
    {
        var hue = (components & ColorComponents.First) != 0 ? color.FirstComponent : 0;
        var saturation = (components & ColorComponents.Second) != 0 ? color.SecondComponent : 100;
        var value = (components & ColorComponents.Third) != 0 ? color.ThirdComponent : 100;


        var hi = (int)(hue / 60) % 6;

        var vmin = (100 - saturation) * value / 100;
        var a = (value - vmin) * ((float)Math.Round(hue) % 60) / 60;
        var vinc = vmin + a;
        var vdec = value - a;

        value *= 2.55f;
        vinc *= 2.55f;
        vmin *= 2.55f;
        vdec *= 2.55f;

        return hi switch
        {
            0 => new Color(value, vinc, vmin),
            1 => new Color(vdec, value, vmin),
            2 => new Color(vmin, value, vinc),
            3 => new Color(vmin, vdec, value),
            4 => new Color(vinc, vmin, value),
            5 => new Color(value, vmin, vdec),
            _ => throw new ArgumentOutOfRangeException(nameof(hue))
        };
    }

    public static Color FromRgb(in Color color)
    {
        const float tolerance = 0.000001f;

        var r = color.FirstComponent;
        var g = color.SecondComponent;
        var b = color.ThirdComponent;

        var max = Math.Max(r, Math.Max(g, b));
        var min = Math.Min(r, Math.Min(g, b));

        var v = max / 2.55f;
        var s = max == 0 ? 0 : (max - min) / max * 100;

        var h = 0;

        if (Math.Abs(max - min) < tolerance)
        {
            return new Color(h, s, v);
        }

        if (Math.Abs(max - r) < tolerance)
        {
            h = (int)((g - b) / (max - min) * 60);
        }
        else if (Math.Abs(max - g) < tolerance)
        {
            h = (int)((b - r) / (max - min) * 60 + 120);
        }
        else if (Math.Abs(max - b) < tolerance)
        {
            h = (int)((r - g) / (max - min) * 60 + 240);
        }

        if (h < 0)
        {
            h += 360;
        }

        return new Color(h, s, v);
    }

    public static void ToRgb(Bitmap bitmap, ColorComponents components = ColorComponents.All)
    {
        bitmap.BytesForColor -= 1;

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
        bitmap.BytesForColor += 1;

        for (int y = 0; y < bitmap.Height; y++)
        {
            for (int x = 0; x < bitmap.Width; x++)
            {
                bitmap.Matrix[y, x] = FromRgb(in bitmap.Matrix[y, x]);
            }
        }
    }
}