using RedPixel.Core.Colors.ValueObjects;
using RedPixel.Core.Models;

namespace RedPixel.Core.Colors;

public class HsvColor : IColorSpace
{
    public static void ToRgb(ref Color color, ColorComponents components = ColorComponents.All)
    {
        var hue = (components & ColorComponents.First) != 0 ? color.FirstComponent : 0;
        var saturation = (components & ColorComponents.Second) != 0 ? color.SecondComponent : 0;
        var value = (components & ColorComponents.Third) != 0 ? color.ThirdComponent : 0;

        var hi = (int)Math.Round(hue / 60) % 6;

        var vmin = (100 - saturation) * value / 100;
        var a = (value - vmin) * ((float)Math.Round(hue) % 60) / 60;
        var vinc = vmin + a;
        var vdec = value - a;

        value *= 2.55f;
        vinc *= 2.55f;
        vmin *= 2.55f;
        vdec *= 2.55f;

        switch (hi)
        {
            case 0:
                color.FirstComponent = value;
                color.SecondComponent = vinc;
                color.ThirdComponent = vmin;
                break;
            case 1:
                color.FirstComponent = vdec;
                color.SecondComponent = value;
                color.ThirdComponent = vmin;
                break;
            case 2:
                color.FirstComponent = vmin;
                color.SecondComponent = value;
                color.ThirdComponent = vinc;
                break;
            case 3:
            {
                color.FirstComponent = vmin;
                color.SecondComponent = vdec;
                color.ThirdComponent = value;
                break;
            }
            case 4:
                color.FirstComponent = vinc;
                color.SecondComponent = vmin;
                color.ThirdComponent = value;
                break;
            case 5:
                color.FirstComponent = value;
                color.SecondComponent = vmin;
                color.ThirdComponent = vdec;
                break;
            default: throw new ArgumentOutOfRangeException(nameof(hue));
        };
    }

    public static void FromRgb(ref Color color)
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
            color.FirstComponent = h;
            color.SecondComponent = s;
            color.ThirdComponent = v;
            return;
        }

        if (Math.Abs(max - r) < tolerance)
        {
            h = (int) ((g - b) / (max - min) * 60);
        }
        else if (Math.Abs(max - g) < tolerance)
        {
            h = (int) ((b - r) / (max - min) * 60 + 120);
        }
        else if (Math.Abs(max - b) < tolerance)
        {
            h = (int) ((r - g) / (max - min) * 60 + 240);
        }

        if (h < 0)
        {
            h += 360;
        }

        color.FirstComponent = h;
        color.SecondComponent = s;
        color.ThirdComponent = v;
    }

    public static void ToRgb(Bitmap bitmap, ColorComponents components = ColorComponents.All)
    {
        bitmap.BytesForColor -= 1;

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
        bitmap.BytesForColor += 1;

        for (int y = 0; y < bitmap.Height; y++)
        {
            for (int x = 0; x < bitmap.Width; x++)
            {
                FromRgb(ref bitmap.Matrix[y, x]);
            }
        }
    }
}