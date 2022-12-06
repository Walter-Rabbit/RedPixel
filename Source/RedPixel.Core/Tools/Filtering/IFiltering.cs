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
}