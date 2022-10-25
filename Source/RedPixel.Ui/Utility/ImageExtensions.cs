using System.Diagnostics;
using System.IO;
using Avalonia;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using RedPixel.Core.Colors;
using RedPixel.Core.ImageParsers;
using Bitmap = Avalonia.Media.Imaging.Bitmap;
using ImageFormat = RedPixel.Core.ImageFormat;
using RedPixelBitmap = RedPixel.Core.Bitmap.Bitmap;

namespace RedPixel.Ui.Utility;

public static class ImageExtensions
{
    public static Bitmap ConvertToAvaloniaBitmap(this RedPixelBitmap bitmap, ColorComponents components = ColorComponents.All)
    {
        using var ms = new MemoryStream();
        bitmap.SelectColorComponents(components);
        ImageParserFactory.CreateParser(ImageFormat.Bmp).SerializeToStream(bitmap, ms, ColorSpace.Rgb);
        ms.Position = 0;
        return new Bitmap(ms);
    }
}