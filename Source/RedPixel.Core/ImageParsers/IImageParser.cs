namespace RedPixel.Core.ImageParsers;

public interface IImageParser
{
    ImageFormat[] ImageFormats { get; }

    Bitmap.Bitmap Parse(Stream content);

    void SerializeToStream(Bitmap.Bitmap image, Stream stream);
}