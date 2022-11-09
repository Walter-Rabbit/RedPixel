using RedPixel.Core.Colors;
using RedPixel.Core.Colors.ValueObjects;

namespace RedPixel.Core.ImageParsers;

public interface IImageParser
{
    ImageFormat[] ImageFormats { get; }

    Bitmap.Bitmap Parse(Stream content, ColorSpace colorSpace);

    void SerializeToStream(
        Bitmap.Bitmap image,
        Stream stream,
        ColorSpace colorSpace,
        ColorComponents components);
}