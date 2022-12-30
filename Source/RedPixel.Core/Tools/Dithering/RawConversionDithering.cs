using RedPixel.Core.Colors.ValueObjects;
using RedPixel.Core.Models;

namespace RedPixel.Core.Tools.Dithering;

public class RawConversionDithering : ADitheringAlgo, IDitheringAlgo
{
    public static void ApplyDithering(Bitmap bitmap, ColorDepth depth)
    {
        for (var y = bitmap.Height - 1; y >= 0; y--)
        for (var x = 0; x < bitmap.Width; x++)
        {
            var pixel = FindClosestPaletteColor(bitmap.Matrix[y, x], depth);
            bitmap.SetPixel(x, y, pixel);
        }
    }
}