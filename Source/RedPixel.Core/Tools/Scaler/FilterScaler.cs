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


                // var x1 = (int)(x * scaleX);
                // var x2 = x1 + 1;
                // var y1 = (int)(y * scaleY);
                // var y2 = y1 + 1;
                //
                //
                //
                // var x1Weight = Filter(xa, 0);
                // var x2Weight = Filter(xa, 1);
                //
                // var y1Weight = Filter(ya, 0);
                // var y2Weight = Filter(ya, 1);
                //
                // x2 = Math.Min(x2, image.Width - 1);
                // y2 = Math.Min(y2, image.Height - 1);
                //
                // var topLeft = image.GetPixel(x1, y1);
                // var topRight = image.GetPixel(x2, y1);
                // var bottomLeft = image.GetPixel(x1, y2);
                // var bottomRight = image.GetPixel(x2, y2);
                //
                // var top = topLeft * x1Weight + topRight * x2Weight;
                // var bottom = bottomLeft * x1Weight + bottomRight * x2Weight;
                // var pixel = top * y1Weight + bottom * y2Weight;

                pixel = pixel.Normalize();

                result.SetPixel(x, y, pixel);

                if (pixel.FirstComponent > 255 || pixel.SecondComponent > 255 || pixel.ThirdComponent > 255)
                    Console.WriteLine();

                if (pixel.FirstComponent < 0 || pixel.SecondComponent < 0 || pixel.ThirdComponent < 0)
                    Console.WriteLine();
            }
        }

        return result;
    }
}