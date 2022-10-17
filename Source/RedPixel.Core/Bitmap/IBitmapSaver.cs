using RedPixel.Core.Colors;

namespace RedPixel.Core.Bitmap;

using ImageFormat = System.Drawing.Imaging.ImageFormat;

public interface IBitmapSaver
{
    ImageFormat ImageFormat { get; }
    void Save(Bitmap bitmap, MemoryStream ms);
}