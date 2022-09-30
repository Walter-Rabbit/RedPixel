using System.Drawing;

namespace RedPixel.Core;

public interface IImageParser
{
    Image Parse(Stream content);

    void SerializeToStream(Image image, Stream stream);

}