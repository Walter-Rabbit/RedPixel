using System.Drawing;
using Bitmap = RedPixel.Core.Models.Bitmap;
using Color = RedPixel.Core.Colors.ValueObjects.Color;

namespace RedPixel.Core.Tools.Filtering;

public class GaussianFiltering : IFiltering
{
    public static Bitmap ApplyFiltering(Bitmap bitmap, float sigma, Point leftTopPoint, Point rightBottomPoint)
    {
        var intSigma = (int)Math.Round(sigma);
        var coreRadius = 3 * intSigma;
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
                GetAreaPixels(bitmap, i, j, intSigma, areaPixels);

                var fc = areaPixels[0].Sum();
                var sc = areaPixels[1].Sum();
                var tc = areaPixels[2].Sum();

                newBitmap.SetPixel(i, j, new Color(fc, sc, tc));
            }
        }

        return newBitmap;
    }

    private static void GetAreaPixels(Bitmap bitmap, int x, int y, int sigma, List<float[]> areaPixels)
    {
        var coreRadius = 3 * sigma;

        var leftX = x - coreRadius;
        var leftY = y - coreRadius;

        var rightX = x + coreRadius;
        var rightY = y + coreRadius;

        for (int i = leftX, kx = 0; i <= rightX; i++, kx++)
        {
            for (int j = leftY, ky = 0; j <= rightY; j++, ky++)
            {
                var pixel = IFiltering.GetClosestPixel(bitmap, i, j);

                var d = Math.Sqrt(Math.Pow(i - x, 2) + Math.Pow(j - y, 2));
                var coefficient = 1f / (2f * Math.PI * sigma * sigma) *
                                  Math.Pow(Math.E, -(d * d) / (2 * sigma * sigma));
                
                var fc = (int)Math.Round(pixel.FirstComponent * coefficient);
                var sc = (int)Math.Round(pixel.SecondComponent * coefficient);
                var tc = (int)Math.Round(pixel.ThirdComponent * coefficient);
                
                var index = 2 * coreRadius * kx + ky;
                areaPixels[0][index] = fc;
                areaPixels[1][index] = sc;
                areaPixels[2][index] = tc;
            }
        }
    }
}