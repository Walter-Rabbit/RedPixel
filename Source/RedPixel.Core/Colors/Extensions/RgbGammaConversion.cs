namespace RedPixel.Core.Colors.Extensions;

public static class RgbGammaConversion
{
    public static RgbColor AssignGamma(this RgbColor rgbColor, float gammaDiffValue = 0f)
    {
        return new RgbColor(
            (float)(255 * Math.Pow(rgbColor.FirstComponent, gammaDiffValue) / Math.Pow(255, gammaDiffValue)),
            (float)(255 * Math.Pow(rgbColor.SecondComponent, gammaDiffValue) / Math.Pow(255, gammaDiffValue)),
            (float)(255 * Math.Pow(rgbColor.ThirdComponent, gammaDiffValue) / Math.Pow(255, gammaDiffValue)),
            rgbColor.BytesForColor);
    }
}