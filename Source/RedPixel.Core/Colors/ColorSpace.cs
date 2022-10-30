using System.Reflection;

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
        (r, g, b, bytesForColor) => new RgbColor(r, g, b, bytesForColor));

    public static ColorSpace Hsl = new(
        "HSL",
        new[] { "H", "S", "L" },
        color => HslColor.FromRgb(color.ToRgb()),
        (h, s, l, bytesForColor) => new HslColor(h, s, l, bytesForColor));

    public static ColorSpace Hsv = new(
        "HSV",
        new[] { "H", "S", "V" },
        color => HsvColor.FromRgb(color.ToRgb()),
        (h, s, v, bytesForColor) => new HsvColor(h, s, v, bytesForColor));

    public static ColorSpace YCoCg = new(
        "YCoCg",
        new[] { "Y", "Co", "Cg" },
        color => YCoCgColor.FromRgb(color.ToRgb()),
        (y, cO, cG, bytesForColor) => new YCoCgColor(y, cO, cG, bytesForColor));

    public static ColorSpace Cmy = new(
        "CMY",
        new[] { "C", "M", "Y" },
        color => CmyColor.FromRgb(color.ToRgb()),
        (c, m, y, bytesForColor) => new CmyColor(c, m, y, bytesForColor));


    public static Lazy<IEnumerable<ColorSpace>> AllSpaces => new(
        () => typeof(ColorSpace)
            .GetFields(BindingFlags.Public | BindingFlags.Static)
            .Where(f => f.FieldType == typeof(ColorSpace))
            .Select(f => (ColorSpace)f.GetValue(null))
    );
}