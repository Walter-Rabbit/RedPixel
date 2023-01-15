using System.IO;
using Avalonia.Media.Imaging;
using RedPixel.Core.Colors;
using RedPixel.Core.Colors.ValueObjects;
using RedPixel.Core.ImageParsers;
using RedPixelBitmap = RedPixel.Core.Models.Bitmap;

namespace RedPixel.Ui.Utility;

public static class ImageExtensions
{
    public static Bitmap ConvertToAvaloniaBitmap(
        this RedPixelBitmap bitmap,
        ColorComponents components = ColorComponents.All)
    {
        using var ms = new MemoryStream();
        BmpImageParser.SerializeToStream(bitmap, ms, ColorSpaces.Rgb, components);
        ms.Position = 0;
        return new Bitmap(ms);
    }
}