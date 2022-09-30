using System.Drawing;

namespace RedPixel.Core;

public static class ImageParserFactory
{
    private static readonly List<IImageParser> Parsers;

    static ImageParserFactory()
    {
        Parsers = new List<IImageParser>()
        {
            new PnmImageParser()
        };
    }

    public static IImageParser CreateParser(Stream content)
    {
        var parser = Parsers.FirstOrDefault(p => p.CanParse(content));

        if (parser is null)
            throw new Exception("Unsupported image format");

        return parser;
    }

    public static IImageParser CreateParser(ImageFormat format)
    {
        var parser = Parsers.FirstOrDefault(p => p.ImageFormats.Contains(format));

        if (parser is null)
            throw new Exception("Unsupported image format");

        return parser;
    }
}