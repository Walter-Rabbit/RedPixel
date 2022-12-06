using System.Drawing;
using Bitmap = RedPixel.Core.Models.Bitmap;
using Color = RedPixel.Core.Colors.ValueObjects.Color;

namespace RedPixel.Core.Tools.Filtering;

public class MedianFiltering : IFiltering
{
    public static Bitmap ApplyFiltering(Bitmap bitmap, float coreRadius, Point leftTopPoint, Point rightBottomPoint)
    {
        var capacity = (int)((2 * coreRadius + 1) * (2 * coreRadius + 1));
        var areaPixels = new List<float[]>
        {
            new float[capacity],
            new float[capacity],
            new float[capacity],
        };
        var newBitmap = new Bitmap(bitmap.Width, bitmap.Height, bitmap.BytesForColor, bitmap.ColorSpace);
        newBitmap.Matrix = bitmap.Matrix.Clone() as Color[,];

        var radius = (int)Math.Round(coreRadius);

        for (var i = leftTopPoint.X; i <= rightBottomPoint.X; i++)
        {
            for (var j = leftTopPoint.Y; j <= rightBottomPoint.Y; j++)
            {
                IFiltering.GetAreaPixels(bitmap, i, j, radius, areaPixels);

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
}