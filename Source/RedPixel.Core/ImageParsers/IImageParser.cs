using RedPixel.Core.Colors;
using RedPixel.Core.Colors.ValueObjects;
using RedPixel.Core.Models;

namespace RedPixel.Core.ImageParsers;

public interface IImageParser
{
    ImageFormat[] ImageFormats { get; }

    Bitmap Parse(Stream content, ColorSpaces colorSpaces);

    void SerializeToStream(Bitmap image, Stream stream, ColorSpaces colorSpaces, ColorComponents components);
}