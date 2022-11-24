using RedPixel.Core.Colors.ValueObjects;
using RedPixel.Core.Models;

namespace RedPixel.Core.Dithering;

public class RandomDithering : IDitheringAlgo
{
    public static void ApplyDithering(Bitmap bitmap)
    {
        var rand = new Random();
        
        for (var y = bitmap.Height - 1; y >= 0; y--)
        {
            for (var x = 0; x < bitmap.Width; x++)
            {
                var pixel = bitmap.Matrix[y, x];

                var bwPixel = (float)(
                    0.0722 * pixel.FirstComponent +
                    0.7152 * pixel.SecondComponent +
                    0.2126 * pixel.ThirdComponent
                );

                bwPixel = bwPixel + 128 - (rand.NextInt64() % 256) > 128 ? 255 : 0;
                
                bitmap.SetPixel(x, y, new Color(bwPixel, bwPixel, bwPixel));
            }
        }
    }
}