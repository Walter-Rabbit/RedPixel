using System.Reflection;
using RedPixel.Core.Tools;

namespace RedPixel.Core;

public class ImageFormat
{
    private readonly Func<Stream, bool> _matchFunc;
    public string Value { get; }
    public string[] Alternatives { get; }

    private ImageFormat(string format, Func<Stream, bool> matchFunc, params string[] alternatives)
    {
        Value = format;
        _matchFunc = matchFunc;
        Alternatives = alternatives;
    }

    public bool IsMatch(string fileExtension)
    {
        return Value == fileExtension || Alternatives.Contains(fileExtension);
    }

    public bool IsMatch(Stream content)
    {
        return _matchFunc(content);
    }

    public static ImageFormat Pnm => new ImageFormat(
        "pnm",
        HeaderMatchFuncFactory.Create(new byte[]{80, 53}, new byte[]{80, 54}),
        "pgm", "ppm");

    public static ImageFormat Bmp => new ImageFormat(
        "bmp",
        HeaderMatchFuncFactory.Create(new byte[]{66, 77}),
        "dib", "rle");

    public static ImageFormat Parse(string fileExtension)
    {
        foreach (var format in AllFormats.Value)
        {
            if (format.IsMatch(fileExtension))
                return format;
        }

        throw new ArgumentOutOfRangeException(nameof(fileExtension), "Unknown image format");
    }

    public static ImageFormat Parse(Stream content)
    {
        foreach (var format in AllFormats.Value)
        {
            if (format._matchFunc(content))
                return format;
        }

        throw new ArgumentOutOfRangeException(nameof(content), "Unknown image format");
    }

    public static Lazy<IEnumerable<ImageFormat>> AllFormats => new (
            () => typeof(ImageFormat)
            .GetProperties(BindingFlags.Public | BindingFlags.Static)
            .Where(f => f.PropertyType == typeof(ImageFormat))
            .Select(f => (ImageFormat)f.GetValue(null))
    );

    protected bool Equals(ImageFormat other)
    {
        return Value == other.Value && Alternatives.SequenceEqual(other.Alternatives);
    }

    public override bool Equals(object obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((ImageFormat)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Value, Alternatives);
    }
}