using RedPixel.Core.Colors.ValueObjects;

namespace RedPixel.Core.Colors;

public class HsvColor : IColor
{
    public ColorComponent FirstComponent { get; }
    public ColorComponent SecondComponent { get; }
    public ColorComponent ThirdComponent { get; }

    public HsvColor(ColorComponent hue, ColorComponent saturation, ColorComponent value)
    {
        FirstComponent = hue;
        SecondComponent = saturation;
        ThirdComponent = value;
    }

    public RgbColor ToRgb(ColorComponents components = ColorComponents.All)
    {
        var hue = (components & ColorComponents.First) != 0 ? FirstComponent.Value : 0;
        var saturation = (components & ColorComponents.Second) != 0 ? SecondComponent.Value : 0;
        var value = (components & ColorComponents.Third) != 0 ? ThirdComponent.Value : 0;

        var hi = (int)Math.Round(hue / 60) % 6;

        var vmin = (100 - saturation) * value / 100;
        var a = (value - vmin) * ((float)Math.Round(hue) % 60) / 60;
        var vinc = vmin + a;
        var vdec = value - a;

        value *= 2.55f;
        vinc *= 2.55f;
        vmin *= 2.55f;
        vdec *= 2.55f;

        var val = new ColorComponent(value, FirstComponent.ByteSize - 1);
        var vin = new ColorComponent(vinc, FirstComponent.ByteSize - 1);
        var vde = new ColorComponent(vdec, FirstComponent.ByteSize - 1);
        var vmi = new ColorComponent(vmin, FirstComponent.ByteSize - 1);

        var rgb = hi switch
        {
            0 => new RgbColor( val, vin, vmi),
            1 => new RgbColor(vde, val, vmi),
            2 => new RgbColor(vmi, val, vin),
            3 => new RgbColor(vmi, vde, val),
            4 => new RgbColor(vin, vmi, val),
            5 => new RgbColor(val, vmi, vde),
            _ => throw new ArgumentOutOfRangeException(nameof(hue))
        };

        return rgb;
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

        var saturation = new ColorComponent(s, rgb.SecondComponent.ByteSize + 1);

        var value = new ColorComponent(v, rgb.ThirdComponent.ByteSize + 1);

        var h = 0;

        if (Math.Abs(max - min) < tolerance)
        {
            var hue = new ColorComponent(h, rgb.FirstComponent.ByteSize + 1);
            return new HsvColor(hue, saturation, value);
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

        {
            var hue = new ColorComponent(h, rgb.FirstComponent.ByteSize + 1);
            return new HsvColor(hue, saturation, value);
        }
    }
}