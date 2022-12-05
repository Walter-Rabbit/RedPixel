using RedPixel.Core.Colors.ValueObjects;
using RedPixel.Core.Models;

namespace RedPixel.Core.Tools.Dithering;

public class OrderDithering : ADitheringAlgo, IDitheringAlgo
{
    private static int[][] pattern =
    {
        new[] { 0, 32, 8, 40, 2, 34, 10, 42 },
        new[] { 48, 16, 56, 24, 50, 18, 58, 26 },
        new[] { 12, 44, 4, 36, 14, 46, 6, 38 },
        new[] { 60, 28, 52, 20, 62, 30, 54, 22 },
        new[] { 3, 35, 11, 43, 1, 33, 9, 41 },
        new[] { 51, 19, 59, 27, 49, 17, 57, 25 },
        new[] { 15, 47, 7, 39, 13, 45, 5, 37 },
        new[] { 63, 31, 55, 23, 61, 29, 53, 21 }
    };

    public static void ApplyDithering(Bitmap bitmap, ColorDepth depth)
    {
        for (var y = 0; y < bitmap.Height; y++)
        {
            for (var x = 0; x < bitmap.Width; x++)
            {
                var patternY = y % 8;
                var patternX = x % 8;

                var oldPixel = bitmap.GetPixel(x, y);
                var r = 255f / depth.FirstComponent;

                var firstComponent = oldPixel.FirstComponent + r * ((float)pattern[patternY][patternX] / 64 - 1f / 2f);
                var secondComponent =
                    oldPixel.SecondComponent + r * ((float)pattern[patternY][patternX] / 64 - 1f / 2f);
                var thirdComponent = oldPixel.ThirdComponent + r * ((float)pattern[patternY][patternX] / 64 - 1f / 2f);


                bitmap.SetPixel(x, y, Normalize(FindClosestPaletteColor(
                    new Color(firstComponent, secondComponent, thirdComponent), depth
                )));
            }
        }
    }
}