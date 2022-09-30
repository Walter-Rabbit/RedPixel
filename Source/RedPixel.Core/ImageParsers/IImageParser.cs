using System.Drawing;

namespace RedPixel.Core;

public interface IImageParser
{
    ImageFormat[] ImageFormats { get; }

    Image Parse(Stream content);

    void SerializeToStream(Image image, Stream stream);
}