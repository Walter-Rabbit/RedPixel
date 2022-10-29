using RedPixel.Core.Colors.ValueObjects;

namespace RedPixel.Core.Colors;

public class HsvColor : IColor
{
    public ColorComponent FirstComponent { get; }
    public ColorComponent SecondComponent { get; }
    public ColorComponent ThirdComponent { get; }

    public HsvColor(float hue, float saturation, float value)
    {
        FirstComponent = new ColorComponent(hue);
        SecondComponent = new ColorComponent(saturation);
        ThirdComponent = new ColorComponent(value);
    }

    public RgbColor ToRgb()
    {
        var hue = FirstComponent.Visible ? FirstComponent.Value : 0;
        var saturation = SecondComponent.Visible ? SecondComponent.Value : 0;
        var value = ThirdComponent.Visible ? ThirdComponent.Value : 0;

        var hi = (int)Math.Round(hue / 60) % 6;

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
            0 => new RgbColor(value, vinc, vmin),
            1 => new RgbColor(vdec, value, vmin),
            2 => new RgbColor(vmin, value, vinc),
            3 => new RgbColor(vmin, vdec, value),
            4 => new RgbColor(vinc, vmin, value),
            5 => new RgbColor(value, vmin, vdec),
            _ => throw new ArgumentOutOfRangeException(nameof(hue))
        };
    }

    public static IColor FromRgb(RgbColor rgb)
    {
        const float tolerance = 0.000001f;
        
        var r = rgb.FirstComponent.Value;
        var g = rgb.SecondComponent.Value;
        var b = rgb.ThirdComponent.Value;

        var max = Math.Max(r, Math.Max(g, b));
        var min = Math.Min(r, Math.Min(g, b));

        var v = max / 2.55f;
        var s = max == 0 ? 0 : (max - min) / max * 100;

        var h = 0;

        if (Math.Abs(max - min) < tolerance)
        {
            return new HsvColor(h, s, v);
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

        return new HsvColor(h, s, v);
    }
}