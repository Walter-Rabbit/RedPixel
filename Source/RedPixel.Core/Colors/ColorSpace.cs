using System.Reflection;

namespace RedPixel.Core.Colors;

public class ColorSpace
{
    public string Name { get; }
    public string[] Components { get;}

    public Func<IColor, IColor> Converter { get; }

    public Func<float, float, float, IColor> Creator { get; }

    private ColorSpace(string name, string[] components, Func<IColor, IColor> converter, Func<float, float, float, IColor> creator)
    {
        Name = name;
        Components = components;
        Converter = converter;
        Creator = creator;
    }

    public static ColorSpace Rgb = new("RGB", new[] { "R", "G", "B" }, color => color.ToRgb(), (r, g, b) => new RgbColor(r, g, b));

    public static ColorSpace Hsv = new("HSV", new[] { "H", "S", "V" }, color => HSVColor.FromRgb(color.ToRgb()), (h, s, v) => new HSVColor(h, s, v));


    public static Lazy<IEnumerable<ColorSpace>> AllSpaces => new (
        () => typeof(ColorSpace)
            .GetFields(BindingFlags.Public | BindingFlags.Static)
            .Where(f => f.FieldType == typeof(ColorSpace))
            .Select(f => (ColorSpace)f.GetValue(null))
    );
}