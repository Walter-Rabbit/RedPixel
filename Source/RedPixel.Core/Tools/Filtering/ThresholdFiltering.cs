using RedPixel.Core.Colors.ValueObjects;
using RedPixel.Core.Models;
using RedPixel.Core.Tools.Utilities;

namespace RedPixel.Core.Tools.Filtering;

public class ThresholdFiltering : IFiltering
{
    public static void ApplyFiltering(Bitmap bitmap, float threshold)
    {
        BwConverter.ConvertToBlackAndWhite(bitmap);

        for (var i = 0; i < bitmap.Width; i++)
        for (var j = 0; j < bitmap.Height; j++)
        {
            var color = bitmap.GetPixel(i, j).FirstComponent > threshold ? 255 : 0;
            bitmap.SetPixel(i, j, new Color(color, color, color));
        }
    }
}