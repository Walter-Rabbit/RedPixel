using RedPixel.Core.Colors.ValueObjects;

namespace RedPixel.Core.Colors;

public class YCoCgColor : IColor
{
    public float FirstComponent { get; }
    public float SecondComponent { get; }
    public float ThirdComponent { get; }
    public int BytesForColor { get; }

    public YCoCgColor(float luma, float cOrange, float cGreen, int bytesForColor)
    {
        FirstComponent = luma;
        SecondComponent = cOrange;
        ThirdComponent = cGreen;
        BytesForColor = bytesForColor;
    }

    public RgbColor ToRgb(ColorComponents components = ColorComponents.All)
    {
        var luma = (components & ColorComponents.First) != 0 ? FirstComponent : 0;
        var cOrange = (components & ColorComponents.Second) != 0 ? SecondComponent : 0;
        var cGreen = (components & ColorComponents.Third) != 0 ? ThirdComponent : 0;

        var y = luma;
        var cO = cOrange - 255;
        var cG = cGreen - 255;

        var r = (y + cO - cG)/2;
        var g = (y + cG)/2;
        var b = (y - cO - cG)/2;

        return new RgbColor(r, g, b, BytesForColor - 1);
    }

    public static IColor FromRgb(RgbColor rgb)
    {
        var r = rgb.FirstComponent * 2;
        var g = rgb.SecondComponent * 2;
        var b = rgb.ThirdComponent * 2;

        var y = (b + 2*g + r)/4;
        var cG = (-b + 2*g - r)/4 + 255;
        var cO = (-b + r) / 2 + 255;

        return new YCoCgColor(y, cO, cG, rgb.BytesForColor + 1);
    }
}