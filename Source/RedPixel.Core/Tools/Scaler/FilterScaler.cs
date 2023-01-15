using RedPixel.Core.Colors.ValueObjects;
using RedPixel.Core.Models;

namespace RedPixel.Core.Tools.Scaler;

public abstract class FilterScaler : IImageScaler
{
    protected abstract float Filter(float a, float point);

    protected abstract int WindowSize { get; }

    public Bitmap Scale(Bitmap image, int width, int height)
    {
        var result = new Bitmap(width, height, image.BytesForColor, image.ColorSpace);
        var scaleX = (float)image.Width / width;
        var scaleY = (float)image.Height / height;


        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                var xs = new int[WindowSize * 2];
                for (int i = 0; i < WindowSize * 2; i++)
                {
                    xs[i] = (int)(x * scaleX) + i + 1 - WindowSize;
                }

                var xa = x * scaleX - (int)(x * scaleX);
                var ya = y * scaleY - (int)(y * scaleY);

                var pixel = new Color(0, 0, 0);

                for (int yi = 0; yi < WindowSize * 2; yi++)
                {
                    var currentY = (int)(y * scaleY) + yi + 1 - WindowSize;
                    currentY = Math.Max(0, Math.Min(currentY, image.Height - 1));
                    var yValue = new Color(0, 0, 0);

                    for (int i = 1 - WindowSize; i <= WindowSize; i++)
                    {
                        var weight = Filter(xa, i);
                        var currentX = xs[i - 1 + WindowSize];
                        currentX = Math.Min(image.Width - 1, currentX);
                        currentX = Math.Max(0, currentX);

                        var color = image.GetPixel(currentX, currentY);
                        yValue += color * weight;
                    }

                    var curYi = yi + 1 - WindowSize;
                    var yWeight = Filter(ya, curYi);
                    pixel += yValue * yWeight;
                }

                pixel = pixel.Normalize();

                result.SetPixel(x, y, pixel);
            }
        }

        return result;
    }
}