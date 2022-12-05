using RedPixel.Core.Colors.ValueObjects;
using RedPixel.Core.Models;

namespace RedPixel.Core.Tools.Filtering;

public class MedianFiltering : IFiltering
{
    public static Bitmap ApplyFiltering(Bitmap bitmap, float coreRadius)
    {
        var capacity = (int)((2 * coreRadius + 1) * (2 * coreRadius + 1));
        var areaPixels = new List<float[]>
        {
            new float[capacity],
            new float[capacity],
            new float[capacity],
        };
        var newBitmap = new Bitmap(bitmap.Width, bitmap.Height, bitmap.BytesForColor, bitmap.ColorSpace);

        var radius = (int)Math.Round(coreRadius);

        for (var i = 0; i < bitmap.Width; i++)
        {
            for (var j = 0; j < bitmap.Height; j++)
            {
                GetAreaPixels(bitmap, i, j, radius, areaPixels);

                Array.Sort(areaPixels[0]);
                Array.Sort(areaPixels[1]);
                Array.Sort(areaPixels[2]);

                var median = capacity / 2;
                var fc = areaPixels[0][median];
                var sc = areaPixels[1][median];
                var tc = areaPixels[2][median];

                newBitmap.SetPixel(i, j, new Color(fc, sc, tc));
            }
        }

        return newBitmap;
    }

    private static void GetAreaPixels(Bitmap bitmap, int x, int y, int radius, List<float[]> areaPixels)
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

    private static Color GetClosestPixel(Bitmap bitmap, int x, int y)
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