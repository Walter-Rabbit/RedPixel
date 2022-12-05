using System.Drawing;
using Bitmap = RedPixel.Core.Models.Bitmap;
using Color = RedPixel.Core.Colors.ValueObjects.Color;

namespace RedPixel.Core.Tools.Filtering;

public class ThresholdFiltering : IFiltering
{
    public static Bitmap ApplyFiltering(Bitmap bitmap, float threshold, Point leftTopPoint, Point rightBottomPoint)
    {
        for (var i = leftTopPoint.X; i <= rightBottomPoint.X; i++)
        {
            for (var j = leftTopPoint.Y; j <= rightBottomPoint.Y; j++)
            {
                var fc = bitmap.GetPixel(i, j).FirstComponent > threshold ? 255 : 0;
                var sc = bitmap.GetPixel(i, j).SecondComponent > threshold ? 255 : 0;
                var tc = bitmap.GetPixel(i, j).ThirdComponent > threshold ? 255 : 0;
                bitmap.SetPixel(i, j, new Color(fc, sc, tc));
            }
        }

        return bitmap;
    }
}