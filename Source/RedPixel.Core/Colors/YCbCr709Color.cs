using RedPixel.Core.Colors.ValueObjects;

namespace RedPixel.Core.Colors;

public class YCbCr709Color : IColor
{
    public float FirstComponent { get; set; }
    public float SecondComponent { get; set; }
    public float ThirdComponent { get; set; }
    public int BytesForColor { get; }


    public YCbCr709Color(float y, float cb, float cr, int bfc)
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
            y + 1.402f * (cr - 128f),
            y - 0.34414f * (cb - 128f) - 0.71414f * (cr - 128f),
            y + 1.772f * (cb - 128f),
            BytesForColor
        );
    }

    public static IColor FromRgb(RgbColor rgb)
    {
        var R = rgb.FirstComponent;
        var G = rgb.SecondComponent;
        var B = rgb.ThirdComponent;


        var y = 0.299f * R + 0.587f * G + 0.114f * B;
        var cb = 128f - 0.168736f * R - 0.331264f * G + 0.5f * B;
        var cr = 128f + 0.5f * R - 0.418688f * G - 0.081312f * B;


        return new YCbCr709Color(y, cb, cr, rgb.BytesForColor);
    }
}