namespace RedPixel.Core.Colors;

public class HSVColor : IColor
{
    public ColorComponent FirstComponent { get; }
    public ColorComponent SecondComponent { get; }
    public ColorComponent ThirdComponent { get; }

    public HSVColor(float hue, float saturation, float value)
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

        var hi = (int)Math.Floor(hue / 60) % 6;

        var vmin = (100 - saturation) * value / 100;
        var a = (value - vmin) * (hue % 60) / 60;
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
        var r = rgb.FirstComponent.Value;
        var g = rgb.SecondComponent.Value;
        var b = rgb.ThirdComponent.Value;

        var max = Math.Max(r, Math.Max(g, b));
        var min = Math.Min(r, Math.Min(g, b));

        var v = max / 2.55f;
        var s = max == 0 ? 0 : (max - min) / max * 100;

        var h = 0;

        if (max == min)
        {
            return new HSVColor(h, s, v);
        }

        if (max == r)
        {
            h = (int) ((g - b) / (max - min) * 60);
        }
        else if (max == g)
        {
            h = (int) ((b - r) / (max - min) * 60 + 120);
        }
        else if (max == b)
        {
            h = (int) ((r - g) / (max - min) * 60 + 240);
        }

        if (h < 0)
        {
            h += 360;
        }

        return new HSVColor(h, s, v);
    }
}