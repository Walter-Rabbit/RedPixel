using RedPixel.Core.Colors.ValueObjects;

namespace RedPixel.Core.Colors.Extensions;

public static class RgbGammaConversion
{
    public static Color AssignGamma(this Color color, float gammaValue)
    {
        var fc = color.FirstComponent / 255f;
        var sc = color.SecondComponent / 255f;
        var tc = color.ThirdComponent / 255f;

        if (gammaValue == 0f)
        {
            if (fc <= 0.0031308f)
            {
                fc = 12.92f * fc;
            }
            else
            {
                fc = 1.055f * (float)Math.Pow(fc, 1f / 2.4f) - 0.055f;
            }

            if (sc <= 0.0031308f)
            {
                sc = 12.92f * sc;
            }
            else
            {
                sc = 1.055f * (float)Math.Pow(sc, 1f / 2.4f) - 0.055f;
            }

            if (tc <= 0.0031308f)
            {
                tc = 12.92f * tc;
            }
            else
            {
                tc = 1.055f * (float)Math.Pow(tc, 1f / 2.4f) - 0.055f;
            }
        }
        else
        {
            fc = (float)Math.Pow(fc, 1f / gammaValue);
            sc = (float)Math.Pow(sc, 1f / gammaValue);
            tc = (float)Math.Pow(tc, 1f / gammaValue);
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
            if (fc <= 0.04045f)
            {
                fc = fc / 12.92f;
            }
            else
            {
                fc = (float)Math.Pow((fc + 0.055f) / 1.055f, 2.4f);
            }

            if (sc <= 0.04045f)
            {
                sc = sc / 12.92f;
            }
            else
            {
                sc = (float)Math.Pow((sc + 0.055f) / 1.055f, 2.4f);
            }

            if (tc <= 0.04045f)
            {
                tc = tc / 12.92f;
            }
            else
            {
                tc = (float)Math.Pow((tc + 0.055f) / 1.055f, 2.4f);
            }
        }
        else
        {
            fc = (float)Math.Pow(fc, targetGammaValue);
            sc = (float)Math.Pow(sc, targetGammaValue);
            tc = (float)Math.Pow(tc, targetGammaValue);
        }

        return new Color(fc * 255f, sc * 255f, tc * 255f);
    }
}