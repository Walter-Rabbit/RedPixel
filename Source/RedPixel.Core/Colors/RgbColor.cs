using RedPixel.Core.Colors.ValueObjects;

namespace RedPixel.Core.Colors;

using SystemColor  = System.Drawing.Color;

public class RgbColor : IColor
{
    public ColorComponent FirstComponent { get; }
    public ColorComponent SecondComponent { get; }
    public ColorComponent ThirdComponent { get; }

    public RgbColor(float r, float g, float b)
    {
        FirstComponent = new ColorComponent(r);
        SecondComponent = new ColorComponent(g);
        ThirdComponent = new ColorComponent(b);
    }

    public RgbColor ToRgb()
    {
        return this;
    }

    public SystemColor ToSystemColor()
    {
        return SystemColor.FromArgb(FirstComponent.ByteValue, SecondComponent.ByteValue, ThirdComponent.ByteValue);
    }

    public static IColor FromRgb(RgbColor rgb)
    {
        return rgb;
    }
}