using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using RedPixel.Core.Bitmap;
using Bitmap = Avalonia.Media.Imaging.Bitmap;
using RedPixelBitmap = RedPixel.Core.Bitmap.Bitmap;

namespace RedPixel.Ui.Utility;

public static class ImageExtensions
{
    public static Bitmap ConvertToAvaloniaBitmap(this Image image)
    {
        using var ms = new MemoryStream();
        image.Save(ms, ImageFormat.Png);
        ms.Position = 0;
        return new Bitmap(ms);
    }

    public static Bitmap ConvertToAvaloniaBitmap(this RedPixelBitmap bitmap)
    {
        using var ms = new MemoryStream();
        BitmapSaverFactory.CreateSaver(ImageFormat.Bmp).Save(bitmap, ms);
        ms.Position = 0;
        return new Bitmap(ms);
    }
}