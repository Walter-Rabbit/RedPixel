using System.Diagnostics;
using System.IO;
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
        var sw = new Stopwatch();
        sw.Start();
        using var ms = new MemoryStream();
        bitmap.SelectColorComponents(components);
        ImageParserFactory.CreateParser(ImageFormat.Bmp).SerializeToStream(bitmap, ms);
        File.AppendAllText("time-log.txt", $"Converted to bmp: {sw.ElapsedMilliseconds} ms\n");
        ms.Position = 0;
        var btm = new Bitmap(ms);
        sw.Stop();
        File.AppendAllText("time-log.txt", $"Converting to Avalonia Bitmap took {sw.ElapsedMilliseconds} ms\n");
        return btm;
    }
}