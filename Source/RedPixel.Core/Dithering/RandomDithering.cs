using RedPixel.Core.Colors.ValueObjects;
using RedPixel.Core.Models;

namespace RedPixel.Core.Dithering;

public class RandomDithering : ADitheringAlgo, IDitheringAlgo
{
    public static void ApplyDithering(Bitmap bitmap, ColorDepth depth)
    {
        var rand = new Random();
        
        for (var y = bitmap.Height - 1; y >= 0; y--)
        {
            for (var x = 0; x < bitmap.Width; x++)
            {
                var pixel = FindClosestPaletteColor(bitmap.Matrix[y, x], depth);

                var delta = 128 - (rand.NextInt64() % 256) > 128 ? 255 : 0;
                
                bitmap.SetPixel(x, y, Normalize(new Color(
                    pixel.FirstComponent + delta, 
                    pixel.SecondComponent + delta, 
                    pixel.ThirdComponent + delta)
                ));
            }
        }
    }
}