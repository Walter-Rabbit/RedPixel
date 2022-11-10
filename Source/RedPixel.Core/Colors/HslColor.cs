using RedPixel.Core.Colors.ValueObjects;
using RedPixel.Core.Models;

namespace RedPixel.Core.Colors;

public class HslColor : IColorSpace
{
    public static void ToRgb(ref Color color, ColorComponents components = ColorComponents.All)
    {
        var hue = (components & ColorComponents.First) != 0 ? color.FirstComponent : 0;
        var saturation = (components & ColorComponents.Second) != 0 ? color.SecondComponent : 0;
        var lightness = (components & ColorComponents.Third) != 0 ? color.ThirdComponent : 0;

        var h = hue / 360;
        var s = saturation / 100;
        var l = lightness / 100;

        var q = l < 0.5 ? l * (1 + s) : l + s - l * s;
        var p = 2 * l - q;

        var trgb = new float[3];
        trgb[0] = h + 1f / 3f;
        trgb[1] = h;
        trgb[2] = h - 1f / 3f;

        var rgb = new float[3];
        for (var i = 0; i < 3; i++)
        {
            switch (trgb[i])
            {
                case < 0:
                    trgb[i] += 1;
                    break;
                case > 1:
                    trgb[i] -= 1;
                    break;
            }

            rgb[i] = trgb[i] switch
            {
                < 1f / 6f => p + (q - p) * 6 * trgb[i],
                >= 1f / 6f and < 1f / 2f => q,
                >= 1f / 2f and < 2f / 3f => p + (q - p) * (2f / 3f - trgb[i]) * 6,
                >= 2f / 3f => p,
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        color.FirstComponent = rgb[0] * 255;
        color.SecondComponent = rgb[1] * 255;
        color.ThirdComponent = rgb[2] * 255;
    }

    public static void FromRgb(ref Color color)
    {
        const float tolerance = 0.000001f;

        var r = color.FirstComponent / 255;
        var g = color.SecondComponent / 255;
        var b = color.ThirdComponent / 255;

        var max = Math.Max(r, Math.Max(g, b));
        var min = Math.Min(r, Math.Min(g, b));

        var l = (max + min) / 2 * 100;
        var s = Math.Abs(max - min) < tolerance ? 0 : (max - min) / (1 - Math.Abs(1 - (max + min))) * 100;

        var h = 0;

        if (Math.Abs(max - min) < tolerance)
        {
            color.FirstComponent = h;
            color.SecondComponent = s;
            color.ThirdComponent = l;
            return;
        }

        if (Math.Abs(max - r) < tolerance && g >= b)
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

        color.FirstComponent = h;
        color.SecondComponent = s;
        color.ThirdComponent = l;
    }

    public static void ToRgb(Bitmap bitmap, ColorComponents components = ColorComponents.All)
    {
        bitmap.BytesForColor--;

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
        bitmap.BytesForColor++;

        for (int y = 0; y < bitmap.Height; y++)
        {
            for (int x = 0; x < bitmap.Width; x++)
            {
                FromRgb(ref bitmap.Matrix[y, x]);
            }
        }
    }
}