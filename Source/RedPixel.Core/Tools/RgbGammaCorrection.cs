using RedPixel.Core.Colors.ValueObjects;

namespace RedPixel.Core.Tools;

public static class RgbGammaCorrection
{
    public static Color AssignGamma(this Color color, float gammaValue)
    {
        var fc = color.FirstComponent / 255f;
        var sc = color.SecondComponent / 255f;
        var tc = color.ThirdComponent / 255f;

        if (gammaValue == 0f)
        {
            fc = fc <= 0.04045f ? fc / 12.92f : (float)Math.Pow((fc + 0.055f) / 1.055f, 2.4f);
            sc = sc <= 0.04045f ? sc / 12.92f : (float)Math.Pow((sc + 0.055f) / 1.055f, 2.4f);
            tc = tc <= 0.04045f ? tc / 12.92f : (float)Math.Pow((tc + 0.055f) / 1.055f, 2.4f);
        }
        else
        {
            fc = (float)Math.Pow(fc, gammaValue);
            sc = (float)Math.Pow(sc, gammaValue);
            tc = (float)Math.Pow(tc, gammaValue);
        }

        return new Color(fc * 255f, sc * 255f, tc * 255f);
    }

    public static Color ConvertToGamma(
        this Color color,
        float fromGammaValue,
        float targetGammaValue)
    {
        var newColor = color.AssignGamma(fromGammaValue);
        var fc = newColor.FirstComponent / 255f;
        var sc = newColor.SecondComponent / 255f;
        var tc = newColor.ThirdComponent / 255f;

        if (targetGammaValue == 0f)
        {
            fc = fc <= 0.0031308f ? 12.92f * fc : 1.055f * (float)Math.Pow(fc, 1f / 2.4f) - 0.055f;
            sc = sc <= 0.0031308f ? 12.92f * sc : 1.055f * (float)Math.Pow(sc, 1f / 2.4f) - 0.055f;
            tc = tc <= 0.0031308f ? 12.92f * tc : 1.055f * (float)Math.Pow(tc, 1f / 2.4f) - 0.055f;
        }
        else
        {
            fc = (float)Math.Pow(fc, 1 / targetGammaValue);
            sc = (float)Math.Pow(sc, 1 / targetGammaValue);
            tc = (float)Math.Pow(tc, 1 / targetGammaValue);
        }

        return new Color(fc * 255f, sc * 255f, tc * 255f);
    }
}