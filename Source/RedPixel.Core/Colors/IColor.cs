using RedPixel.Core.Colors.ValueObjects;

namespace RedPixel.Core.Colors;

public interface IColor
{
    public float FirstComponent { get; set; }
    public float SecondComponent { get; set; }
    public float ThirdComponent { get; set; }
    public int BytesForColor { get; }

    RgbColor ToRgb(ColorComponents components = ColorComponents.All);

    static abstract IColor FromRgb(RgbColor rgb);
}