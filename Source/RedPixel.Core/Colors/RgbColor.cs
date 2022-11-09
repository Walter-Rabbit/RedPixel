using RedPixel.Core.Colors.ValueObjects;

namespace RedPixel.Core.Colors;

using SystemColor = System.Drawing.Color;

public class RgbColor : IColor
{
    public float FirstComponent { get; }
    public float SecondComponent { get; }
    public float ThirdComponent { get; }
    public int BytesForColor { get; }

    public RgbColor(float r, float g, float b, int bytesForColor)
    {
        FirstComponent = r;
        SecondComponent = g;
        ThirdComponent = b;
        BytesForColor = bytesForColor;
    }

    public RgbColor ToRgb(ColorComponents components = ColorComponents.All)
    {
        if (components == ColorComponents.All)
            return this;

        var r = (components & ColorComponents.First) != 0 ? FirstComponent : 0;
        var g = (components & ColorComponents.Second) != 0 ? SecondComponent : 0;
        var b = (components & ColorComponents.Third) != 0 ? ThirdComponent : 0;

        return new RgbColor(r, g, b, BytesForColor);
    }

    public static IColor FromRgb(RgbColor rgb)
    {
        return rgb;
    }
}