using RedPixel.Core.Colors.ValueObjects;
using RedPixel.Core.Models;

namespace RedPixel.Core.Dithering;

public class FloydSteinbergDithering : ADitheringAlgo,  IDitheringAlgo
{
    public static void ApplyDithering(Bitmap bitmap)
    {
        for (var y = 0; y < bitmap.Height; y++)
        {
            for (var x = 0; x < bitmap.Width; x++)
            {
                var oldPixel = bitmap.GetPixel(x, y);
                var newPixel = FindClosestPaletteColor(oldPixel);
                bitmap.SetPixel(x, y, newPixel);

                var quantError = GetError(oldPixel, newPixel);
                
                if (x < bitmap.Width - 1)
                    bitmap.SetPixel(x + 1, y, GetPixelWithError(bitmap.GetPixel(x + 1, y), quantError, 7f/16));
                
                if (x > 0 && y < bitmap.Height - 1)
                    bitmap.SetPixel(x - 1, y + 1, GetPixelWithError(bitmap.GetPixel(x - 1, y + 1), quantError, 3f/16));
                
                if (y < bitmap.Height - 1)
                    bitmap.SetPixel(x, y + 1, GetPixelWithError(bitmap.GetPixel(x, y + 1), quantError, 5f/16));
                
                if (x < bitmap.Width - 1 && y < bitmap.Height - 1)
                    bitmap.SetPixel(x + 1, y + 1, GetPixelWithError(bitmap.GetPixel(x + 1, y + 1), quantError, 1f/16));
            }
        }
    }
}