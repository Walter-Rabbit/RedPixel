using RedPixel.Core.Colors.ValueObjects;
using RedPixel.Core.Models;

namespace RedPixel.Core.Dithering.utils;

public class GradientGenerator
{
    public static void Generate(Bitmap bitmap, int height, int width)
    {
        bitmap = new Bitmap(width, height, bitmap.BytesForColor, bitmap.ColorSpace);
        
        for (var y = 0; y < bitmap.Height; y++)
        {
            for (var x = 0; x < bitmap.Width; x++)
            {
                var bwColor = 255f * x / bitmap.Height;
                bitmap.SetPixel(x, y, new Color(bwColor, bwColor, bwColor));
            }
        }
    }
}