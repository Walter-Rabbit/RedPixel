using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using Bitmap = Avalonia.Media.Imaging.Bitmap;

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
}