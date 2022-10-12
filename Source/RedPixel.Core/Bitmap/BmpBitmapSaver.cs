using System.Drawing;

namespace RedPixel.Core.Bitmap;

using ImageFormat = System.Drawing.Imaging.ImageFormat;
public class BmpBitmapSaver : IBitmapSaver
{
    public ImageFormat ImageFormat => ImageFormat.Bmp;

    public void Save(Bitmap bitmap, MemoryStream ms)
    {
        var img = bitmap.GetSystemBitmap();
        img.Save(ms, ImageFormat.Bmp);
    }
}