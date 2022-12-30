using RedPixel.Core.Colors;
using RedPixel.Core.Colors.ValueObjects;
using RedPixel.Core.Models;

namespace RedPixel.Core.Tools.Utilities;

public class GradientGenerator
{
    public static Bitmap Generate(int height, int width, int bytesForColor, ColorSpaces colorSpace)
    {
        var bitmap = new Bitmap(width, height, bytesForColor, colorSpace);

        for (var y = 0; y < bitmap.Height; y++)
        for (var x = 0; x < bitmap.Width; x++)
        {
            var bwColor = 255f * x / bitmap.Width;
            bitmap.SetPixel(x, y, new Color(bwColor, bwColor, bwColor));
        }

        return bitmap;
    }
}