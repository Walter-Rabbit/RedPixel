namespace RedPixel.Core;

public class ImageParserFactory
{
    public static IImageParser Create(ImageFormat format)
    {
        return format.Value switch
        {
            ".pnm" => new PnmImageParser(),
            _ => throw new ArgumentOutOfRangeException(nameof(format))
        };
    }
}