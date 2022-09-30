using System.Collections;
using System.Reflection;

namespace RedPixel.Core;

public class ImageFormat
{
    public string Value { get; }
    public string[] Alternatives { get; }

    private ImageFormat(string format, params string[] alternatives)
    {
        Value = format;
        Alternatives = alternatives;
    }

    public bool IsMatch(string fileExtension)
    {
        return Value == fileExtension || Alternatives.Contains(fileExtension);
    }

    public static ImageFormat Pnm => new ImageFormat(".pnm", ".pgm", ".ppm");

    public static ImageFormat Parse(string fileExtension)
    {
        foreach (var format in AllFormats.Value)
        {
            if (format.IsMatch(fileExtension))
                return format;
        }

        throw new ArgumentOutOfRangeException(nameof(fileExtension), "Unknown image format");
    }

    public static Lazy<IEnumerable<ImageFormat>> AllFormats => new (
            () => typeof(ImageFormat)
            .GetProperties(BindingFlags.Public | BindingFlags.Static)
            .Where(f => f.PropertyType == typeof(ImageFormat))
            .Select(f => (ImageFormat)f.GetValue(null))
    );
}