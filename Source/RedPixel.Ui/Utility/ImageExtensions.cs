using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using RedPixel.Core.Bitmap;
using RedPixel.Core.Colors;
using Bitmap = Avalonia.Media.Imaging.Bitmap;
using RedPixelBitmap = RedPixel.Core.Bitmap.Bitmap;

namespace RedPixel.Ui.Utility;

public static class ImageExtensions
{
    public static Bitmap ConvertToAvaloniaBitmap(this RedPixelBitmap bitmap, ColorComponents components = ColorComponents.All)
    {
        using var ms = new MemoryStream();
        if (components != ColorComponents.All)
            bitmap = bitmap.SelectColorComponents(components);
        BitmapSaverFactory.CreateSaver(ImageFormat.Bmp).Save(bitmap, ms);
        ms.Position = 0;
        return new Bitmap(ms);
    }
}