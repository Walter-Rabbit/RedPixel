using RedPixel.Core.Colors.ValueObjects;

namespace RedPixel.Core.Colors;

public class CmyColor : IColor
{
    public ColorComponent FirstComponent { get; }
    public ColorComponent SecondComponent { get; }
    public ColorComponent ThirdComponent { get; }

    public CmyColor(ColorComponent cyan, ColorComponent magenta, ColorComponent yellow)
    {
        FirstComponent = cyan;
        SecondComponent = magenta;
        ThirdComponent = yellow;
    }

    public RgbColor ToRgb(ColorComponents components = ColorComponents.All)
    {
        var r = FirstComponent with
        {
            Value = (components & ColorComponents.First) != 0 ? 255 - FirstComponent.Value : 0
        };

        var g = SecondComponent with
        {
            Value = (components & ColorComponents.Second) != 0 ? 255 - SecondComponent.Value : 0
        };

        var b = ThirdComponent with
        {
            Value = (components & ColorComponents.Third) != 0 ? 255 - ThirdComponent.Value : 0
        };

        return new RgbColor(r, g, b);
    }

    public static IColor FromRgb(RgbColor rgb)
    {
        var c = rgb.FirstComponent with
        {
            Value = 255 - rgb.FirstComponent.Value
        };
        var m = rgb.SecondComponent with
        {
            Value = 255 - rgb.SecondComponent.Value
        };
        var y = rgb.ThirdComponent with
        {
            Value = 255 - rgb.ThirdComponent.Value
        };

        return new CmyColor(c, m, y);
    }
}