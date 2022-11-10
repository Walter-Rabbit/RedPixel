using System.Diagnostics;
using System.IO;
using Avalonia;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using RedPixel.Core.Colors;
using RedPixel.Core.Colors.ValueObjects;
using RedPixel.Core.ImageParsers;
using SkiaSharp;
using Bitmap = Avalonia.Media.Imaging.Bitmap;
using ImageFormat = RedPixel.Core.ImageFormat;
using RedPixelBitmap = RedPixel.Core.Models.Bitmap;

namespace RedPixel.Ui.Utility;

public static class ImageExtensions
{
    public static Bitmap ConvertToAvaloniaBitmap(this RedPixelBitmap bitmap, ColorComponents components = ColorComponents.All)
    {
        using var ms = new MemoryStream();
        ImageParserFactory.CreateParser(ImageFormat.Bmp).SerializeToStream(bitmap, ms, ColorSpaces.Rgb, components);
        ms.Position = 0;
        return new Bitmap(ms);
    }
}