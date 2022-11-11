using RedPixel.Core.Colors.ValueObjects;

namespace RedPixel.Core.Colors.Extensions;

public static class RgbGammaConversion
{
    public static Color AssignGamma(this Color color, float maxValue, float gammaCoefficient)
    {
        var fc = (float)(maxValue * Math.Pow(color.FirstComponent / maxValue, gammaCoefficient));
        var sc = (float)(maxValue * Math.Pow(color.SecondComponent / maxValue, gammaCoefficient));
        var tc = (float)(maxValue * Math.Pow(color.ThirdComponent / maxValue, gammaCoefficient));

        return new Color(fc, sc, tc);
    }

    public static Color ConvertToGammaAndAssign(this Color color, float maxValue, float gammaCoefficient)
    {
        var fc = (float)Math.Pow(color.FirstComponent / maxValue, 1f / gammaCoefficient) * maxValue;
        var sc = (float)Math.Pow(color.SecondComponent / maxValue, 1f / gammaCoefficient) * maxValue;
        var tc = (float)Math.Pow(color.ThirdComponent / maxValue, 1f / gammaCoefficient) * maxValue;

        fc = (float)(maxValue * Math.Pow(fc / maxValue, gammaCoefficient));
        sc = (float)(maxValue * Math.Pow(sc / maxValue, gammaCoefficient));
        tc = (float)(maxValue * Math.Pow(tc / maxValue, gammaCoefficient));

        return new Color(fc, sc, tc);
    }
}