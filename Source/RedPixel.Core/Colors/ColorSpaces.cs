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

    public ColorSpaces(string name, string[] components, InFunc<Color, ColorComponents> colorToRgb, InFunc<Color> colorFromRgb, Action<Bitmap, ColorComponents> bitmapToRgb, Action<Bitmap> bitmapFromRgb)
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
        RgbColor.ToRgb,
        RgbColor.FromRgb,
        RgbColor.ToRgb,
        RgbColor.FromRgb);

    public static ColorSpaces Hsl = new(
        "HSL",
        new[] { "H", "S", "L" },
        HslColor.ToRgb,
        HslColor.FromRgb,
        HslColor.ToRgb,
        HslColor.FromRgb);

    public static ColorSpaces Hsv = new(
        "HSV",
        new[] { "H", "S", "V" },
        HsvColor.ToRgb,
        HsvColor.FromRgb,
        HsvColor.ToRgb,
        HsvColor.FromRgb);

    public static ColorSpaces YCoCg = new(
        "YCoCg",
        new[] { "Y", "Co", "Cg" },
        YCoCgColor.ToRgb,
        YCoCgColor.FromRgb,
        YCoCgColor.ToRgb,
        YCoCgColor.FromRgb);

    public static ColorSpaces Cmy = new(
        "CMY",
        new[] { "C", "M", "Y" },
        CmyColor.ToRgb,
        CmyColor.FromRgb,
        CmyColor.ToRgb,
        CmyColor.FromRgb);

    public static ColorSpaces YCbCr601 = new(
        "YCbCr601",
        new[] { "Y", "Cb", "Cr" },
        YCbCr601Color.ToRgb,
        YCbCr601Color.FromRgb,
        YCbCr601Color.ToRgb,
        YCbCr601Color.FromRgb);

    public static ColorSpaces YCbCr709 = new(
        "YCbCr709",
        new[] { "Y", "Cb", "Cr" },
        YCbCr709Color.ToRgb,
        YCbCr709Color.FromRgb,
        YCbCr709Color.ToRgb,
        YCbCr709Color.FromRgb);

    public static Lazy<IEnumerable<ColorSpaces>> AllSpaces => new(
        () => typeof(ColorSpaces)
            .GetFields(BindingFlags.Public | BindingFlags.Static)
            .Where(f => f.FieldType == typeof(ColorSpaces))
            .Select(f => (ColorSpaces)f.GetValue(null))
    );
}