using System.Drawing;

namespace RedPixel.Core;

public interface IImageParser
{
    ImageFormat[] ImageFormats { get; }

    // TODO : maybe move to ImageFormat?
    bool CanParse(Stream content);

    Image Parse(Stream content);

    void SerializeToStream(Image image, Stream stream);
}