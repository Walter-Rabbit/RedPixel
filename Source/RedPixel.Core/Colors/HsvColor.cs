using RedPixel.Core.Colors.ValueObjects;

namespace RedPixel.Core.Colors;

public class HsvColor : IColor
{
    public float FirstComponent { get; }
    public float SecondComponent { get; }
    public float ThirdComponent { get; }
    public int BytesForColor { get; }

    public HsvColor(float hue, float saturation, float value, int bytesForColor)
    {
        FirstComponent = hue;
        SecondComponent = saturation;
        ThirdComponent = value;
        BytesForColor = bytesForColor;
    }

    public RgbColor ToRgb(ColorComponents components = ColorComponents.All)
    {
        var hue = (components & ColorComponents.First) != 0 ? FirstComponent : 0;
        var saturation = (components & ColorComponents.Second) != 0 ? SecondComponent : 100;
        var value = (components & ColorComponents.Third) != 0 ? ThirdComponent : 100;

        var hi = (int)(hue / 60) % 6;

        var vmin = (100 - saturation) * value / 100;
        var a = (value - vmin) * ((float)Math.Round(hue) % 60) / 60;
        var vinc = vmin + a;
        var vdec = value - a;

        value *= 2.55f;
        vinc *= 2.55f;
        vmin *= 2.55f;
        vdec *= 2.55f;

        var rgb = hi switch
        {
            0 => new RgbColor(value, vinc, vmin, BytesForColor - 1),
            1 => new RgbColor(vdec, value, vmin, BytesForColor - 1),
            2 => new RgbColor(vmin, value, vinc, BytesForColor - 1),
            3 => new RgbColor(vmin, vdec, value, BytesForColor - 1),
            4 => new RgbColor(vinc, vmin, value, BytesForColor - 1),
            5 => new RgbColor(value, vmin, vdec, BytesForColor - 1),
            _ => throw new ArgumentOutOfRangeException(nameof(hue))
        };

        return rgb;
    }

    public static IColor FromRgb(RgbColor rgb)
    {
        const float tolerance = 0.000001f;

        var r = rgb.FirstComponent;
        var g = rgb.SecondComponent;
        var b = rgb.ThirdComponent;

        var max = Math.Max(r, Math.Max(g, b));
        var min = Math.Min(r, Math.Min(g, b));

        var v = max / 2.55f;
        var s = max == 0 ? 0 : (max - min) / max * 100;

        var h = 0;

        if (Math.Abs(max - min) < tolerance)
        {
            return new HsvColor(h, s, v, rgb.BytesForColor + 1);
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

        {
            return new HsvColor(h, s, v, rgb.BytesForColor + 1);
        }
    }
}