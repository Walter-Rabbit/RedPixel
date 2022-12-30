using RedPixel.Core.Models;

namespace RedPixel.Core.Tools.Scaler;

public class NearestNeighbourScaler : IImageScaler
{
    public Bitmap Scale(Bitmap image, int width, int height)
    {
        var result = new Bitmap(width, height, image.BytesForColor, image.ColorSpace);

        var scaleX = (double)image.Width / width;
        var scaleY = (double)image.Height / height;

        for (var y = 0; y < height; y++)
        {
            for (var x = 0; x < width; x++)
            {
                var sourceX = (int)(x * scaleX);
                var sourceY = (int)(y * scaleY);

                result.SetPixel(x, y, image.GetPixel(sourceX, sourceY));
            }
        }

        return result;
    }
}