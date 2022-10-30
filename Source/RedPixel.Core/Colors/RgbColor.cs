using RedPixel.Core.Colors.ValueObjects;

namespace RedPixel.Core.Colors;

using SystemColor = System.Drawing.Color;

public class RgbColor : IColor
{
    public ColorComponent FirstComponent { get; }
    public ColorComponent SecondComponent { get; }
    public ColorComponent ThirdComponent { get; }
    public int BytesForColor { get; }

    public RgbColor(float r, float g, float b, int bytesForColor)
    {
        FirstComponent = new ColorComponent(r);
        SecondComponent = new ColorComponent(g);
        ThirdComponent = new ColorComponent(b);
        BytesForColor = bytesForColor;
    }

    public RgbColor ToRgb()
    {
        return this;
    }

    public SystemColor ToSystemColor()
    {
        return SystemColor.FromArgb(
            BitConverter.ToInt32(FirstComponent.BytesValue),
            BitConverter.ToInt32(SecondComponent.BytesValue),
            BitConverter.ToInt32(ThirdComponent.BytesValue));
    }

    public static IColor FromRgb(RgbColor rgb)
    {
        return rgb;
    }
}