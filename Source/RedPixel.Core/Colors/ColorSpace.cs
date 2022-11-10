using System.Reflection;
using RedPixel.Core.Colors.ValueObjects;

namespace RedPixel.Core.Colors;

public class ColorSpace
{
    public string Name { get; }
    public string[] Components { get; }

    public Func<IColor, IColor> Converter { get; }

    public Func<float, float, float, int, IColor> Creator { get; }

    private ColorSpace(
        string name,
        string[] components,
        Func<IColor, IColor> converter,
        Func<float, float, float, int, IColor> creator)
    {
        Name = name;
        Components = components;
        Converter = converter;
        Creator = creator;
    }

    public static ColorSpace Rgb = new(
        "RGB",
        new[] { "R", "G", "B" },
        color => color.ToRgb(),
        (r, g, b, bfc) => new RgbColor(r, g, b, bfc));

    public static ColorSpace YCbCr601 = new(
        "YCbCr601",
        new[] { "Y", "Cb", "Cr" },
        color => color is YCbCr601Color ? color : YCbCr601Color.FromRgb(color.ToRgb()),
        (y, cb, cr, bfc) => new YCbCr601Color(y, cb, cr, bfc));

    public static ColorSpace YCbCr709 = new(
        "YCbCr709",
        new[] { "Y", "Cb", "Cr" },
        color => color is YCbCr709Color ? color : YCbCr709Color.FromRgb(color.ToRgb()),
        (y, cb, cr, bfc) => new YCbCr709Color(y, cb, cr, bfc));

    public static ColorSpace Hsl = new(
        "HSL",
        new[] { "H", "S", "L" },
        color => color is HslColor ? color : HslColor.FromRgb(color.ToRgb()),
        (h, s, l, bfc) => new HslColor(h, s, l, bfc));

    public static ColorSpace Hsv = new(
        "HSV",
        new[] { "H", "S", "V" },
        color => HsvColor.FromRgb(color.ToRgb()),
        (h, s, v, bfc) => new HsvColor(h, s, v, bfc));

    public static ColorSpace YCoCg = new(
        "YCoCg",
        new[] { "Y", "Co", "Cg" },
        color => YCoCgColor.FromRgb(color.ToRgb()),
        (y, cO, cG, bfc) => new YCoCgColor(y, cO, cG, bfc));

    public static ColorSpace Cmy = new(
        "CMY",
        new[] { "C", "M", "Y" },
        color => CmyColor.FromRgb(color.ToRgb()),
        (c, m, y, bfc) => new CmyColor(c, m, y, bfc));


    public static Lazy<IEnumerable<ColorSpace>> AllSpaces => new(
        () => typeof(ColorSpace)
            .GetFields(BindingFlags.Public | BindingFlags.Static)
            .Where(f => f.FieldType == typeof(ColorSpace))
            .Select(f => (ColorSpace)f.GetValue(null))
    );
}