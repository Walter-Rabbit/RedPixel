using RedPixel.Core.Colors.ValueObjects;

namespace RedPixel.Core.Colors;
public interface IColor
{
    public ColorComponent FirstComponent { get; }
    public ColorComponent SecondComponent { get; }
    public ColorComponent ThirdComponent { get; }
    public int BytesForColor => FirstComponent.ByteSize;

    RgbColor ToRgb(ColorComponents components = ColorComponents.All);

    static abstract IColor FromRgb(RgbColor rgb);
}