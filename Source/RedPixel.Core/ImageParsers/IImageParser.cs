using System.Drawing;

namespace RedPixel.Core.ImageParsers;

public interface IImageParser
{
    ImageFormat[] ImageFormats { get; }

    Image Parse(Stream content);

    void SerializeToStream(Image image, Stream stream);
}