using System.IO;
using RedPixel.Core.Colors;
using RedPixel.Core.Colors.ValueObjects;
using RedPixel.Core.ImageParsers;
using Bitmap = Avalonia.Media.Imaging.Bitmap;
using ImageFormat = RedPixel.Core.ImageFormat;
using RedPixelBitmap = RedPixel.Core.Bitmap.Bitmap;

namespace RedPixel.Ui.Utility;

public static class ImageExtensions
{
    public static Bitmap ConvertToAvaloniaBitmap(
        this RedPixelBitmap bitmap,
        float gammaValue = 0,
        ColorComponents components = ColorComponents.All)
    {
        using var ms = new MemoryStream();
        (ImageParserFactory.CreateParser(ImageFormat.Bmp) as BmpImageParser)?
            .GetBmpStreamForAvalonia(bitmap, ms, ColorSpace.Rgb, components, gammaValue);
        ms.Position = 0;
        return new Bitmap(ms);
    }
}