namespace RedPixel.Core.Colors.Extensions;

public static class RgbGammaConversion
{
    public static IColor AssignGamma(this IColor color, float fromGammaValue, float targetGammaValue = 0f)
    {
        fromGammaValue = fromGammaValue == 0f ? 1 : fromGammaValue;
        var gammaCoefficient = targetGammaValue == 0f ? 1f / fromGammaValue : targetGammaValue / fromGammaValue;
        var maxValue = (float)Math.Pow(2f, 8f * color.BytesForColor) - 1f;

        color.FirstComponent = (float)(maxValue * Math.Pow(color.FirstComponent / maxValue, gammaCoefficient));
        color.SecondComponent = (float)(maxValue * Math.Pow(color.SecondComponent / maxValue, gammaCoefficient));
        color.ThirdComponent = (float)(maxValue * Math.Pow(color.ThirdComponent / maxValue, gammaCoefficient));

        return color;
    }

    public static IColor ConvertToGamma(this IColor color, float fromGammaValue, float targetGammaValue = 0f)
    {
        fromGammaValue = fromGammaValue == 0f ? 1 : fromGammaValue;
        var gammaCoefficient = targetGammaValue == 0f ? 1f / fromGammaValue : targetGammaValue / fromGammaValue;
        var maxValue = (float)Math.Pow(2f, 8f * color.BytesForColor) - 1f;

        color.FirstComponent = (float)Math.Pow(color.FirstComponent / maxValue, 1f / gammaCoefficient) * maxValue;
        color.SecondComponent = (float)Math.Pow(color.SecondComponent / maxValue, 1f / gammaCoefficient) * maxValue;
        color.ThirdComponent = (float)Math.Pow(color.ThirdComponent / maxValue, 1f / gammaCoefficient) * maxValue;

        return color;
    }
}