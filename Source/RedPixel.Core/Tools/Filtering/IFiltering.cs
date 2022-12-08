using System.Drawing;
using Bitmap = RedPixel.Core.Models.Bitmap;
using Color = RedPixel.Core.Colors.ValueObjects.Color;

namespace RedPixel.Core.Tools.Filtering;

public interface IFiltering
{
    static abstract Bitmap ApplyFiltering(Bitmap bitmap, float parameter, Point leftTopPoint, Point rightBottomPoint);

    protected static void GetAreaPixels(
        Bitmap bitmap,
        int x,
        int y,
        int radius,
        List<float[]> areaPixels,
        Point leftTopPoint,
        Point rightBottomPoint)
    {
        var leftX = x - radius;
        var leftY = y - radius;

        var rightX = x + radius;
        var rightY = y + radius;

        for (int i = leftX, kx = 0; i <= rightX; i++, kx++)
        {
            for (int j = leftY, ky = 0; j <= rightY; j++, ky++)
            {
                var pixel = GetClosestPixel(bitmap, i, j, leftTopPoint, rightBottomPoint);
                var index = 2 * radius * kx + ky;
                areaPixels[0][index] = pixel.FirstComponent;
                areaPixels[1][index] = pixel.SecondComponent;
                areaPixels[2][index] = pixel.ThirdComponent;
            }
        }
    }

    protected static Color GetClosestPixel(Bitmap bitmap, int x, int y, Point leftTopPoint, Point rightBottomPoint)
    {
        if (x < leftTopPoint.X)
        {
            x = leftTopPoint.X;
        }
        else if (x > rightBottomPoint.X)
        {
            x = rightBottomPoint.X;
        }

        if (y < leftTopPoint.Y)
        {
            y = leftTopPoint.Y;
        }
        else if (y > rightBottomPoint.Y)
        {
            y = rightBottomPoint.Y;
        }

        return bitmap.GetPixel(x, y);
    }

    protected static Bitmap Convolution(Bitmap bitmap, float[,] kernel, Point leftTopPoint, Point rightBottomPoint)
    {
        var newBitmap = new Bitmap(bitmap.Width, bitmap.Height, bitmap.BytesForColor, bitmap.ColorSpace)
        {
            Matrix = bitmap.Matrix.Clone() as Color[,]
        };

        var kernelWidth = kernel.GetLength(0);
        var kernelHeight = kernel.GetLength(1);

        for (var x = leftTopPoint.X; x <= rightBottomPoint.X; x++)
        {
            for (var y = leftTopPoint.Y; y <= rightBottomPoint.Y; y++)
            {
                float fcSum = 0, scSum = 0, tcSum = 0, kSum = 0;

                for (var i = 0; i < kernelWidth; i++)
                {
                    for (var j = 0; j < kernelHeight; j++)
                    {
                        var pixelPosX = x + (i - kernelWidth / 2);
                        var pixelPosY = y + (j - kernelHeight / 2);
                        if (pixelPosX < 0 || pixelPosX >= bitmap.Width || pixelPosY < 0 || pixelPosY >= bitmap.Height)
                        {
                            continue;
                        }

                        var pixel = bitmap.GetPixel(pixelPosX, pixelPosY);
                        var kernelVal = kernel[i, j];

                        fcSum += pixel.FirstComponent * kernelVal;
                        scSum += pixel.SecondComponent * kernelVal;
                        tcSum += pixel.ThirdComponent * kernelVal;

                        kSum += kernelVal;
                    }
                }

                if (kSum <= 0)
                {
                    kSum = 1;
                }

                fcSum /= kSum;
                fcSum = fcSum < 0 ? 0 : fcSum;
                fcSum = fcSum > 255 ? 255 : fcSum;

                scSum /= kSum;
                scSum = scSum < 0 ? 0 : scSum;
                scSum = scSum > 255 ? 255 : scSum;

                tcSum /= kSum;
                tcSum = tcSum < 0 ? 0 : tcSum;
                tcSum = tcSum > 255 ? 255 : tcSum;

                var newPixel = new Color(fcSum, scSum, tcSum);
                newBitmap.SetPixel(x, y, newPixel);
            }
        }

        return newBitmap;
    }
}