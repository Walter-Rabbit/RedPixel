using RedPixel.Core.Models;

namespace RedPixel.Core.Tools.Scaler;

public interface IImageScaler
{
    Bitmap Scale(Bitmap image, int width, int height);
}