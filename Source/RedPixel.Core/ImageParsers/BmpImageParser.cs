namespace RedPixel.Core.ImageParsers;

using ImageFormat = System.Drawing.Imaging.ImageFormat;
public class BmpImageParser : IImageParser
{
    public Core.ImageFormat[] ImageFormats => new[] { Core.ImageFormat.Bmp, };

    public Bitmap.Bitmap Parse(Stream content)
    {
        throw new NotImplementedException();
    }

    public void SerializeToStream(Bitmap.Bitmap image, Stream stream)
    {
        var img = image.GetSystemBitmap();
        img.Save(stream, ImageFormat.Bmp);
    }
}