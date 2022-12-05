using RedPixel.Core.Colors.ValueObjects;
using RedPixel.Core.Models;

namespace RedPixel.Core.Tools.Filtering;

public class ThresholdFiltering : IFiltering
{
    public static Bitmap ApplyFiltering(Bitmap bitmap, float threshold)
    {
        for (var i = 0; i < bitmap.Width; i++)
        {
            for (var j = 0; j < bitmap.Height; j++)
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