using RedPixel.Core.Colors.ValueObjects;

namespace RedPixel.Core.Colors;

public class CmyColor : IColor
{
    public ColorComponent FirstComponent { get; }
    public ColorComponent SecondComponent { get; }
    public ColorComponent ThirdComponent { get; }

    public CmyColor(float cyan, float magenta, float yellow)
    {
        FirstComponent = new ColorComponent(cyan);
        SecondComponent = new ColorComponent(magenta);
        ThirdComponent = new ColorComponent(yellow);
    }
    
    public RgbColor ToRgb()
    {
        var c = FirstComponent.Visible ? FirstComponent.Value : 0;
        var m = SecondComponent.Visible ? SecondComponent.Value : 0;
        var y = ThirdComponent.Visible ? ThirdComponent.Value : 0;


        var r = 255 - c;
        var g = 255 - m;
        var b = 255 - y;

        return new RgbColor(r, g, b);
    }

    public static IColor FromRgb(RgbColor rgb)
    {
        var r = rgb.FirstComponent.Value;
        var g = rgb.SecondComponent.Value;
        var b = rgb.ThirdComponent.Value;

        var c = 255 - r;
        var m = 255 - g;
        var y = 255 - b;

        return new CmyColor(c, m, y);
    }
}