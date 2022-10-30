using RedPixel.Core.Colors.ValueObjects;

namespace RedPixel.Core.Colors;
public interface IColor
{
    public ColorComponent FirstComponent { get; }
    public ColorComponent SecondComponent { get; }
    public ColorComponent ThirdComponent { get; }
    public int BytesForColor { get; }

    void SelectComponents(ColorComponents components)
    {
        FirstComponent.Visible = (components & ColorComponents.First) != 0;
        SecondComponent.Visible = (components & ColorComponents.Second) != 0;
        ThirdComponent.Visible = (components & ColorComponents.Third) != 0;
    }

    RgbColor ToRgb();

    System.Drawing.Color ToSystemColor() => ToRgb().ToSystemColor();

    static abstract IColor FromRgb(RgbColor rgb);
}