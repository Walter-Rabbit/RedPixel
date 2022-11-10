namespace RedPixel.Core.Colors.Extensions;

public static class RgbGammaConversion
{
    public static IColor AssignGamma(this IColor color, float gammaValue = 0f)
    {
        if (gammaValue == 0f)
        {
            return color;
        }

        var maxValue = (float)Math.Pow(2f, 8f * color.BytesForColor) - 1f;

        return new RgbColor(
            (float)(maxValue * Math.Pow(color.FirstComponent / maxValue, gammaValue)),
            (float)(maxValue * Math.Pow(color.SecondComponent / maxValue, gammaValue)),
            (float)(maxValue * Math.Pow(color.ThirdComponent / maxValue, gammaValue)),
            color.BytesForColor);
    }

    public static IColor ConvertToGamma(this IColor color, float gammaValue = 0f)
    {
        if (gammaValue == 0f)
        {
            return color;
        }

        var maxValue = (float)Math.Pow(2f, 8f * color.BytesForColor) - 1f;
        
        color.FirstComponent = (float)Math.Pow(color.FirstComponent / maxValue, 1f / gammaValue) * maxValue;
        color.SecondComponent = (float)Math.Pow(color.SecondComponent / maxValue, 1f / gammaValue) * maxValue;
        color.ThirdComponent = (float)Math.Pow(color.ThirdComponent / maxValue, 1f / gammaValue) * maxValue;

        return color;
    }
}