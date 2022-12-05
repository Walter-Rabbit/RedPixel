using RedPixel.Core.Colors.ValueObjects;
using RedPixel.Core.Models;

namespace RedPixel.Core.Tools.Utilities;

public class BwConverter
{
    public static void ConvertToBlackAndWhite(Bitmap bitmap)
    {
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

                bitmap.SetPixel(x, y, new Color(bwPixel, bwPixel, bwPixel));
            }
        }
    }
}