using RedPixel.Core.Colors.ValueObjects;

namespace RedPixel.Core.Colors.Extensions;

public static class RgbGammaConversion
{
    public static Color AssignGamma(this Color color, int bytesForColor, float fromGammaValue, float targetGammaValue = 0f)
    {
        fromGammaValue = fromGammaValue == 0f ? 1 : fromGammaValue;
        var gammaCoefficient = targetGammaValue == 0f ? 1f / fromGammaValue : targetGammaValue / fromGammaValue;
        var maxValue = (float)Math.Pow(2f, 8f * bytesForColor) - 1f;

        var fc = (float)(maxValue * Math.Pow(color.FirstComponent / maxValue, gammaCoefficient));
        var sc = (float)(maxValue * Math.Pow(color.SecondComponent / maxValue, gammaCoefficient));
        var tc = (float)(maxValue * Math.Pow(color.ThirdComponent / maxValue, gammaCoefficient));

        return new Color(fc, sc, tc);
    }

    public static Color ConvertToGamma(this Color color, int bytesForColor, float fromGammaValue, float targetGammaValue = 0f)
    {
        fromGammaValue = fromGammaValue == 0f ? 1 : fromGammaValue;
        var gammaCoefficient = targetGammaValue == 0f ? 1f / fromGammaValue : targetGammaValue / fromGammaValue;
        var maxValue = (float)Math.Pow(2f, 8f * bytesForColor) - 1f;

        var fc = (float)Math.Pow(color.FirstComponent / maxValue, 1f / gammaCoefficient) * maxValue;
        var sc = (float)Math.Pow(color.SecondComponent / maxValue, 1f / gammaCoefficient) * maxValue;
        var tc = (float)Math.Pow(color.ThirdComponent / maxValue, 1f / gammaCoefficient) * maxValue;

        return new Color(fc, sc, tc);
    }
}