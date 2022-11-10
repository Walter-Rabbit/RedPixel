using RedPixel.Core.Colors.ValueObjects;

namespace RedPixel.Core.Colors;

public class CmyColor : IColor
{
    public float FirstComponent { get; set; }
    public float SecondComponent { get; set; }
    public float ThirdComponent { get; set; }
    public int BytesForColor { get; }

    public CmyColor(float cyan, float magenta, float yellow, int bytesForColor)
    {
        FirstComponent = cyan;
        SecondComponent = magenta;
        ThirdComponent = yellow;
        BytesForColor = bytesForColor;
    }

    public RgbColor ToRgb(ColorComponents components = ColorComponents.All)
    {
        var r = (components & ColorComponents.First) != 0 ? 255 - FirstComponent : 0;

        var g = (components & ColorComponents.Second) != 0 ? 255 - SecondComponent : 0;

        var b = (components & ColorComponents.Third) != 0 ? 255 - ThirdComponent : 0;

        return new RgbColor(r, g, b, BytesForColor);
    }

    public static IColor FromRgb(RgbColor rgb)
    {
        var c = 255 - rgb.FirstComponent;
        var m = 255 - rgb.SecondComponent;
        var y = 255 - rgb.ThirdComponent;

        return new CmyColor(c, m, y, rgb.BytesForColor);
    }
}