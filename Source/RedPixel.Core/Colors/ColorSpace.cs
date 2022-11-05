using System.Reflection;
using RedPixel.Core.Colors.ValueObjects;

namespace RedPixel.Core.Colors;

public class ColorSpace
{
    public string Name { get; }
    public string[] Components { get; }

    public Func<IColor, IColor> Converter { get; }

    public Func<ColorComponent, ColorComponent, ColorComponent, IColor> Creator { get; }

    private ColorSpace(
        string name,
        string[] components,
        Func<IColor, IColor> converter,
        Func<ColorComponent, ColorComponent, ColorComponent, IColor> creator)
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
        (r, g, b) => new RgbColor(r, g, b));

    public static ColorSpace Hsl = new(
        "HSL",
        new[] { "H", "S", "L" },
        color => color is HslColor ? color : HslColor.FromRgb(color.ToRgb()),
        (h, s, l) => new HslColor(h, s, l));

    public static ColorSpace Hsv = new(
        "HSV",
        new[] { "H", "S", "V" },
        color => HsvColor.FromRgb(color.ToRgb()),
        (h, s, v) => new HsvColor(h, s, v));

    public static ColorSpace YCoCg = new(
        "YCoCg",
        new[] { "Y", "Co", "Cg" },
        color => YCoCgColor.FromRgb(color.ToRgb()),
        (y, cO, cG) => new YCoCgColor(y, cO, cG));

    public static ColorSpace Cmy = new(
        "CMY",
        new[] { "C", "M", "Y" },
        color => CmyColor.FromRgb(color.ToRgb()),
        (c, m, y) => new CmyColor(c, m, y));


    public static Lazy<IEnumerable<ColorSpace>> AllSpaces => new(
        () => typeof(ColorSpace)
            .GetFields(BindingFlags.Public | BindingFlags.Static)
            .Where(f => f.FieldType == typeof(ColorSpace))
            .Select(f => (ColorSpace)f.GetValue(null))
    );
}