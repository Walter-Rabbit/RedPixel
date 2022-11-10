using RedPixel.Core.Colors.ValueObjects;

namespace RedPixel.Core.Colors;

public class YCbCr601Color : IColor
{
    public float FirstComponent { get; }
    public float SecondComponent { get; }
    public float ThirdComponent { get; }
    public int BytesForColor { get; }

    public YCbCr601Color(float y, float cb, float cr, int bfc)
    {
        FirstComponent = y;
        SecondComponent = cb;
        ThirdComponent = cr;
        BytesForColor = bfc;
    }

    public RgbColor ToRgb(ColorComponents components = ColorComponents.All)
    {
        var y = (components & ColorComponents.First) != 0 ? FirstComponent : 0;
        var cb = (components & ColorComponents.Second) != 0 ? SecondComponent : 128;
        var cr = (components & ColorComponents.Third) != 0 ? ThirdComponent : 128;

        return new RgbColor(
            298.082f / 256 * y + 408.583f / 256 * cr - 222.921f,
            296.082f / 256 * y - 100.291f / 256 * cb - 208.120f / 256 * cr + 135.576f,
            298.082f / 256 * y + 516.412f / 256 * cb - 276.836f,
            BytesForColor
        );
    }

    public static IColor FromRgb(RgbColor rgb)
    {
        var r = rgb.FirstComponent;
        var g = rgb.SecondComponent;
        var b = rgb.ThirdComponent;

        var y = 16 + (65.738f * r + 129.057f * g + 25.064f * b) / 256;
        var cb = 128 + (-37.945f * r - 74.494f * g + 112.439f * b) / 256;
        var cr = 128 + (112.439f * r - 94.154f * g - 18.285f * b) / 256;

        return new YCbCr601Color(y, cb, cr, rgb.BytesForColor);
    }
}