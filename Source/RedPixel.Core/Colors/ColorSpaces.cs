using System.Reflection;
using RedPixel.Core.Colors.ValueObjects;
using RedPixel.Core.Models;

namespace RedPixel.Core.Colors;

public class ColorSpaces
{
    public delegate void RefAction<T>(ref T arg);
    public delegate void RefAction<TFirst, TSecond>(ref TFirst arg, TSecond secondArg);

    public string Name { get; }
    public string[] Components { get; }

    public RefAction<Color, ColorComponents> ColorToRgb { get; }

    public RefAction<Color> ColorFromRgb { get; }

    public Action<Bitmap, ColorComponents> BitmapToRgb { get; }

    public Action<Bitmap> BitmapFromRgb { get; }

    public ColorSpaces(string name, string[] components, RefAction<Color, ColorComponents> colorToRgb, RefAction<Color> colorFromRgb, Action<Bitmap, ColorComponents> bitmapToRgb, Action<Bitmap> bitmapFromRgb)
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


    public static Lazy<IEnumerable<ColorSpaces>> AllSpaces => new(
        () => typeof(ColorSpaces)
            .GetFields(BindingFlags.Public | BindingFlags.Static)
            .Where(f => f.FieldType == typeof(ColorSpaces))
            .Select(f => (ColorSpaces)f.GetValue(null))
    );
}