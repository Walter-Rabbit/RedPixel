using RedPixel.Core.Colors.ValueObjects;

namespace RedPixel.Core.Colors;

public class HslColor : IColor
{
    public ColorComponent FirstComponent { get; }
    public ColorComponent SecondComponent { get; }
    public ColorComponent ThirdComponent { get; }

    public HslColor(ColorComponent hue, ColorComponent saturation, ColorComponent lightness)
    {
        FirstComponent = hue;
        SecondComponent = saturation;
        ThirdComponent = lightness;
    }

    public RgbColor ToRgb(ColorComponents components = ColorComponents.All)
    {
        var hue = (components & ColorComponents.First) != 0 ? FirstComponent.Value : 0;
        var saturation = (components & ColorComponents.Second) != 0 ? SecondComponent.Value : 0;
        var lightness = (components & ColorComponents.Third) != 0 ? ThirdComponent.Value : 0;

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

        var r = FirstComponent with
        {
            Value = rgb[0] * 255,
            ByteSize = FirstComponent.ByteSize - 1
        };
        var g = SecondComponent with
        {
            Value = rgb[1] * 255,
            ByteSize = SecondComponent.ByteSize - 1
        };
        var b = ThirdComponent with
        {
            Value = rgb[2] * 255,
            ByteSize = ThirdComponent.ByteSize - 1
        };

        return new RgbColor(r, g, b);
    }

    public static IColor FromRgb(RgbColor rgb)
    {
        const float tolerance = 0.000001f;

        var r = rgb.FirstComponent.Value / 255;
        var g = rgb.SecondComponent.Value / 255;
        var b = rgb.ThirdComponent.Value / 255;

        var max = Math.Max(r, Math.Max(g, b));
        var min = Math.Min(r, Math.Min(g, b));

        var l = (max + min) / 2 * 100;
        var s = Math.Abs(max - min) < tolerance ? 0 : (max - min) / (1 - Math.Abs(1 - (max + min))) * 100;

        var h = 0;

        var saturation = rgb.SecondComponent with
        {
            Value = s,
            ByteSize = rgb.SecondComponent.ByteSize + 1
        };

        var lightness = rgb.ThirdComponent with
        {
            Value = l,
            ByteSize = rgb.ThirdComponent.ByteSize + 1
        };

        if (Math.Abs(max - min) < tolerance)
        {
            var hue = rgb.FirstComponent with
            {
                Value = h,
                ByteSize = rgb.FirstComponent.ByteSize + 1
            };

            return new HslColor(hue, saturation, lightness);
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

        {
            var hue = rgb.FirstComponent with
            {
                Value = h,
                ByteSize = rgb.FirstComponent.ByteSize + 1
            };

            return new HslColor(hue, saturation, lightness);
        }
    }
}