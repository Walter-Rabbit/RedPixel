namespace RedPixel.Core.ImageParsers;

public static class ImageParserFactory
{
    private static readonly List<IImageParser> Parsers;

    static ImageParserFactory()
    {
        Parsers = new List<IImageParser>
        {
            new PnmImageParser(),
            new PngImageParser()
        };
    }

    public static IImageParser CreateParser(ImageFormat format)
    {
        var parser = Parsers.FirstOrDefault(p => p.ImageFormats.Contains(format));

        if (parser is null)
            throw new Exception("Unsupported image format");

        return parser;
    }
}