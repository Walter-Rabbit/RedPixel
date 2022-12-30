using RedPixel.Core.Colors.ValueObjects;
using RedPixel.Core.Models;

namespace RedPixel.Core.Tools.Dithering;

public class AtkinsonDithering : ADitheringAlgo, IDitheringAlgo
{
    public static void ApplyDithering(Bitmap bitmap, ColorDepth depth)
    {
        for (var y = 0; y < bitmap.Height; y++)
        for (var x = 0; x < bitmap.Width; x++)
        {
            var oldPixel = bitmap.GetPixel(x, y);
            var newPixel = FindClosestPaletteColor(oldPixel, depth);
            bitmap.SetPixel(x, y, newPixel);

            var quantError = GetError(oldPixel, newPixel);

            if (x < bitmap.Width - 1)
                bitmap.SetPixel(x + 1, y, GetPixelWithError(bitmap.GetPixel(x + 1, y), quantError, 1f / 8));

            if (x < bitmap.Width - 2)
                bitmap.SetPixel(x + 2, y, GetPixelWithError(bitmap.GetPixel(x + 2, y), quantError, 1f / 8));

            if (y < bitmap.Height - 1)
                bitmap.SetPixel(x, y + 1, GetPixelWithError(bitmap.GetPixel(x, y + 1), quantError, 1f / 8));

            if (y < bitmap.Height - 2)
                bitmap.SetPixel(x, y + 2, GetPixelWithError(bitmap.GetPixel(x, y + 2), quantError, 1f / 8));

            if (x > 0 && y < bitmap.Height - 1)
                bitmap.SetPixel(x - 1, y + 1, GetPixelWithError(bitmap.GetPixel(x - 1, y + 1), quantError, 1f / 8));


            if (x < bitmap.Width - 1 && y < bitmap.Height - 1)
                bitmap.SetPixel(x + 1, y + 1, GetPixelWithError(bitmap.GetPixel(x + 1, y + 1), quantError, 1f / 8));
        }
    }
}