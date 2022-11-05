using RedPixel.Core.Colors.ValueObjects;

namespace RedPixel.Core.Colors;

using SystemColor = System.Drawing.Color;

public class RgbColor : IColor
{
    public ColorComponent FirstComponent { get; }
    public ColorComponent SecondComponent { get; }
    public ColorComponent ThirdComponent { get; }

    public RgbColor(ColorComponent r, ColorComponent g, ColorComponent b)
    {
        FirstComponent = r;
        SecondComponent = g;
        ThirdComponent = b;
    }

    public RgbColor ToRgb(ColorComponents components = ColorComponents.All)
    {
        if (components == ColorComponents.All)
            return this;

        var r = (components & ColorComponents.First) != 0 ? FirstComponent.Value : 0;
        var g = (components & ColorComponents.Second) != 0 ? SecondComponent.Value : 0;
        var b = (components & ColorComponents.Third) != 0 ? ThirdComponent.Value : 0;

        return new RgbColor(
            new ColorComponent(r, FirstComponent.ByteSize),
            new ColorComponent(g, SecondComponent.ByteSize),
            new ColorComponent(b, ThirdComponent.ByteSize));
    }

    public static IColor FromRgb(RgbColor rgb)
    {
        return rgb;
    }
}