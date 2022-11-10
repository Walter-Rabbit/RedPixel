using RedPixel.Core.Colors.ValueObjects;

namespace RedPixel.Core.Colors;

public interface IColor
{
    public float FirstComponent { get; }
    public float SecondComponent { get; }
    public float ThirdComponent { get; }
    public int BytesForColor { get; }

    RgbColor ToRgb(ColorComponents components = ColorComponents.All);

    static abstract IColor FromRgb(RgbColor rgb);
}