using System.Drawing;
using Bitmap = RedPixel.Core.Models.Bitmap;
using Color = RedPixel.Core.Colors.ValueObjects.Color;

namespace RedPixel.Core.Tools.Filtering;

public interface IFiltering
{
    static abstract Bitmap ApplyFiltering(Bitmap bitmap, float parameter, Point leftTopPoint, Point rightBottomPoint);
    
    protected static void GetAreaPixels(Bitmap bitmap, int x, int y, int radius, List<float[]> areaPixels)
    {
        var leftX = x - radius;
        var leftY = y - radius;

        var rightX = x + radius;
        var rightY = y + radius;

        for (int i = leftX, kx = 0; i <= rightX; i++, kx++)
        {
            for (int j = leftY, ky = 0; j <= rightY; j++, ky++)
            {
                var pixel = GetClosestPixel(bitmap, i, j);
                var index = 2 * radius * kx + ky;
                areaPixels[0][index] = pixel.FirstComponent;
                areaPixels[1][index] = pixel.SecondComponent;
                areaPixels[2][index] = pixel.ThirdComponent;
            }
        }
    }
    
    protected static Color GetClosestPixel(Bitmap bitmap, int x, int y)
    {
        if (x < 0)
        {
            x = 0;
        }
        else if (x >= bitmap.Width)
        {
            x = bitmap.Width - 1;
        }

        if (y < 0)
        {
            y = 0;
        }
        else if (y >= bitmap.Height)
        {
            y = bitmap.Height - 1;
        }

        return bitmap.GetPixel(x, y);
    }
}