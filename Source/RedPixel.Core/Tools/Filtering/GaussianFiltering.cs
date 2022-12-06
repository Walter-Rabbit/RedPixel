using System.Drawing;
using Bitmap = RedPixel.Core.Models.Bitmap;
using Color = RedPixel.Core.Colors.ValueObjects.Color;

namespace RedPixel.Core.Tools.Filtering;

public class GaussianFiltering : IFiltering
{
    public static Bitmap ApplyFiltering(Bitmap bitmap, float sigma, Point leftTopPoint, Point rightBottomPoint)
    {
        var coreRadius = (int)Math.Round(3 * sigma);
        var capacity = (2 * coreRadius + 1) * (2 * coreRadius + 1);
        var areaPixels = new List<float[]>
        {
            new float[capacity],
            new float[capacity],
            new float[capacity],
        };
        var newBitmap = new Bitmap(bitmap.Width, bitmap.Height, bitmap.BytesForColor, bitmap.ColorSpace);
        newBitmap.Matrix = bitmap.Matrix.Clone() as Color[,];

        for (var i = leftTopPoint.X; i <= rightBottomPoint.X; i++)
        {
            for (var j = leftTopPoint.Y; j <= rightBottomPoint.Y; j++)
            {
                GetAreaPixels(bitmap, i, j, coreRadius, sigma, areaPixels, leftTopPoint, rightBottomPoint);

                var fc = areaPixels[0].Sum();
                var sc = areaPixels[1].Sum();
                var tc = areaPixels[2].Sum();

                newBitmap.SetPixel(i, j, new Color(fc, sc, tc));
            }
        }

        return newBitmap;
    }

    private static void GetAreaPixels(
        Bitmap bitmap,
        int x,
        int y,
        int radius,
        float sigma,
        List<float[]> areaPixels,
        Point leftTopPoint,
        Point rightBottomPoint)
    {
        var leftX = x - radius;
        var leftY = y - radius;

        var rightX = x + radius;
        var rightY = y + radius;

        var coefficient1 = 1f / (2f * Math.PI * sigma * sigma);
        var coefficient3 = 2 * sigma * sigma;

        for (int i = leftX, kx = 0; i <= rightX; i++, kx++)
        {
            for (int j = leftY, ky = 0; j <= rightY; j++, ky++)
            {
                var pixel = IFiltering.GetClosestPixel(bitmap, i, j, leftTopPoint, rightBottomPoint);

                var d = Math.Sqrt(Math.Pow(i - x, 2) + Math.Pow(j - y, 2));
                var coefficient = coefficient1 * Math.Pow(Math.E, -(d * d) / coefficient3);

                var fc = (int)Math.Round(pixel.FirstComponent * coefficient);
                var sc = (int)Math.Round(pixel.SecondComponent * coefficient);
                var tc = (int)Math.Round(pixel.ThirdComponent * coefficient);

                var index = 2 * radius * kx + ky;
                areaPixels[0][index] = fc;
                areaPixels[1][index] = sc;
                areaPixels[2][index] = tc;
            }
        }
    }
}