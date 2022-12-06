
using System.Drawing;
using Bitmap = RedPixel.Core.Models.Bitmap;
using Color = RedPixel.Core.Colors.ValueObjects.Color;

namespace RedPixel.Core.Tools.Filtering;

public class BoxBlurFiltering : IFiltering
{
    public static Bitmap ApplyFiltering(Bitmap bitmap, float coreRadius, Point leftTopPoint, Point rightBottomPoint)
    {
        var radius = (int)Math.Round(coreRadius);
        var capacity = (2 * radius + 1) * (2 * radius + 1);
        var areaPixels = new List<float[]>
        {
            new float[capacity],
            new float[capacity],
            new float[capacity],
        };
        var newBitmap = new Bitmap(bitmap.Width, bitmap.Height, bitmap.BytesForColor, bitmap.ColorSpace);
        newBitmap.Matrix = bitmap.Matrix.Clone() as Color[,];

        var coefficient = 1f / capacity;
        
        for (var i = leftTopPoint.X; i <= rightBottomPoint.X; i++)
        {
            for (var j = leftTopPoint.Y; j <= rightBottomPoint.Y; j++)
            {
                IFiltering.GetAreaPixels(bitmap, i, j, radius, areaPixels, leftTopPoint, rightBottomPoint);

                var fc = areaPixels[0].Sum() * coefficient;
                var sc = areaPixels[1].Sum() * coefficient;
                var tc = areaPixels[2].Sum() * coefficient;

                newBitmap.SetPixel(i, j, new Color(fc, sc, tc));
            }
        }

        return newBitmap;
    }
}