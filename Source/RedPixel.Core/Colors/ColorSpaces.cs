using System.Reflection;
using RedPixel.Core.Colors.ValueObjects;
using RedPixel.Core.Models;

namespace RedPixel.Core.Colors;

public class ColorSpaces
{
    //TODO: FIX
    public delegate Color InFunc<T>(in T arg);

    public delegate Color InFunc<TFirst, TSecond>(in TFirst arg, TSecond secondArg);

    public string Name { get; }
    public string[] Components { get; }

    public InFunc<Color, ColorComponents> ColorToRgb { get; }

    public InFunc<Color> ColorFromRgb { get; }

    public Action<Bitmap, ColorComponents> BitmapToRgb { get; }

    public Action<Bitmap> BitmapFromRgb { get; }

    public ColorSpaces(
        string name,
        string[] components,
        InFunc<Color, ColorComponents> colorToRgb,
        InFunc<Color> colorFromRgb,
        Action<Bitmap, ColorComponents> bitmapToRgb,
        Action<Bitmap> bitmapFromRgb)
    {
        Name = name;
        Components = components;
        ColorToRgb = colorToRgb;
        ColorFromRgb = colorFromRgb;
        BitmapToRgb = bitmapToRgb;
        BitmapFromRgb = bitmapFromRgb;
    }

    public static ColorSpaces Rgb = new(
        "RGB",
        new[] { "R", "G", "B" },
        RgbColorSpace.ToRgb,
        RgbColorSpace.FromRgb,
        RgbColorSpace.ToRgb,
        RgbColorSpace.FromRgb);

    public static ColorSpaces Hsl = new(
        "HSL",
        new[] { "H", "S", "L" },
        HslColorSpace.ToRgb,
        HslColorSpace.FromRgb,
        HslColorSpace.ToRgb,
        HslColorSpace.FromRgb);

    public static ColorSpaces Hsv = new(
        "HSV",
        new[] { "H", "S", "V" },
        HsvColorSpace.ToRgb,
        HsvColorSpace.FromRgb,
        HsvColorSpace.ToRgb,
        HsvColorSpace.FromRgb);

    public static ColorSpaces YCoCg = new(
        "YCoCg",
        new[] { "Y", "Co", "Cg" },
        YCoCgColorSpace.ToRgb,
        YCoCgColorSpace.FromRgb,
        YCoCgColorSpace.ToRgb,
        YCoCgColorSpace.FromRgb);

    public static ColorSpaces Cmy = new(
        "CMY",
        new[] { "C", "M", "Y" },
        CmyColorSpace.ToRgb,
        CmyColorSpace.FromRgb,
        CmyColorSpace.ToRgb,
        CmyColorSpace.FromRgb);

    public static ColorSpaces YCbCr601 = new(
        "YCbCr601",
        new[] { "Y", "Cb", "Cr" },
        YCbCr601ColorSpace.ToRgb,
        YCbCr601ColorSpace.FromRgb,
        YCbCr601ColorSpace.ToRgb,
        YCbCr601ColorSpace.FromRgb);

    public static ColorSpaces YCbCr709 = new(
        "YCbCr709",
        new[] { "Y", "Cb", "Cr" },
        YCbCr709ColorSpace.ToRgb,
        YCbCr709ColorSpace.FromRgb,
        YCbCr709ColorSpace.ToRgb,
        YCbCr709ColorSpace.FromRgb);

    public static Lazy<IEnumerable<ColorSpaces>> AllSpaces => new(
        () => typeof(ColorSpaces)
            .GetFields(BindingFlags.Public | BindingFlags.Static)
            .Where(f => f.FieldType == typeof(ColorSpaces))
            .Select(f => (ColorSpaces)f.GetValue(null))
    );
}