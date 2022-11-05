using RedPixel.Core.Colors.ValueObjects;

namespace RedPixel.Core.Colors;

public class YCoCgColor : IColor
{
    public ColorComponent FirstComponent { get; }
    public ColorComponent SecondComponent { get; }
    public ColorComponent ThirdComponent { get; }

    public YCoCgColor(ColorComponent luma, ColorComponent cOrange, ColorComponent cGreen)
    {
        FirstComponent = luma;
        SecondComponent = cOrange;
        ThirdComponent = cGreen;
    }

    public RgbColor ToRgb(ColorComponents components = ColorComponents.All)
    {
        var luma = (components & ColorComponents.First) != 0 ? FirstComponent.Value : 0;
        var cOrange = (components & ColorComponents.Second) != 0 ? SecondComponent.Value : 0;
        var cGreen = (components & ColorComponents.Third) != 0 ? ThirdComponent.Value : 0;

        var y = luma;
        var cO = cOrange - 255;
        var cG = cGreen - 255;

        var r = (y + cO - cG)/2;
        var g = (y + cG)/2;
        var b = (y - cO - cG)/2;

        var red = new ColorComponent(r, FirstComponent.ByteSize-1);
        var green = new ColorComponent(g, SecondComponent.ByteSize-1);
        var blue = new ColorComponent(b, ThirdComponent.ByteSize-1);

        return new RgbColor(red, green, blue);
    }

    public static IColor FromRgb(RgbColor rgb)
    {
        var r = rgb.FirstComponent.Value * 2;
        var g = rgb.SecondComponent.Value * 2;
        var b = rgb.ThirdComponent.Value * 2;

        var y = (b + 2*g + r)/4;
        var cG = (-b + 2*g - r)/4 + 255;
        var cO = (-b + r) / 2 + 255;

        var luma = new ColorComponent(Value: y, ByteSize: rgb.FirstComponent.ByteSize + 1);
        var cOrange = new ColorComponent(Value: cO, ByteSize: rgb.SecondComponent.ByteSize + 1);
        var cGreen = new ColorComponent(Value: cG, ByteSize: rgb.ThirdComponent.ByteSize + 1);

        return new YCoCgColor(luma, cOrange, cGreen);
    }
}