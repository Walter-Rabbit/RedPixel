using RedPixel.Core.Colors.ValueObjects;

namespace RedPixel.Core.Tools.Utilities;

public class Normalizer
{
    public static Color Normalize(Color color)
    {
        return new Color(
            ((int)color.FirstComponent + 512) % 256,
            ((int)color.SecondComponent + 512) % 256,
            ((int)color.ThirdComponent + 512) % 256
        );
    }
}