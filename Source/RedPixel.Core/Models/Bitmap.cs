using RedPixel.Core.Colors;
using RedPixel.Core.Colors.ValueObjects;
using RedPixel.Core.Tools;

namespace RedPixel.Core.Models;

public class Bitmap
{
    public Bitmap(int width, int height, int bytesForColor, ColorSpaces colorSpace)
    {
        ColorSpace = colorSpace;
        BytesForColor = bytesForColor;
        Matrix = new Color[height, width];
    }

    public Color[,] Matrix { get; set; }

    public ColorSpaces ColorSpace { get; private set; }

    public int Width => Matrix.Length == 0 ? 0 : Matrix.GetLength(1);
    public int Height => Matrix.GetLength(0);
    public int BytesForColor { get; set; }

    public float Gamma { get; set; } = 1;

    public void SetPixel(int x, int y, Color clr)
    {
        Matrix[y, x] = clr;
    }

    public Color GetPixel(int x, int y)
    {
        return Matrix[y, x];
    }

    public void ToColorSpace(ColorSpaces colorSpace)
    {
        if (colorSpace == ColorSpace)
            return;

        ColorSpace.BitmapToRgb(this, ColorComponents.All);
        colorSpace.BitmapFromRgb(this);

        ColorSpace = colorSpace;
    }

    public Bitmap ConvertToGamma(float targetGammaValue)
    {
        for (var y = 0; y < Height; y++)
        for (var x = 0; x < Width; x++)
            Matrix[y, x] = Matrix[y, x].ConvertToGamma(Gamma, targetGammaValue);

        Gamma = targetGammaValue;
        return this;
    }

    public double[][] GetHistogram(int fromX, int toX, int fromY, int toY)
    {
        var histogramValues = new double[3][];

        for (int i = 0; i < 3; i++)
        {
            histogramValues[i] = new double[BytesForColor * 256];
            histogramValues[i].AsSpan().Fill(0);
        }


        for (int y = fromY; y < toY; y++)
        {
            for (int x = fromX; x < toX; x++)
            {
                histogramValues[0][(int)Matrix[y, x].FirstComponent]++;
                histogramValues[1][(int)Matrix[y, x].SecondComponent]++;
                histogramValues[2][(int)Matrix[y, x].ThirdComponent]++;
            }
        }

        return histogramValues;
    }

    public void ApplyContrastAdjustment(float ignorePart)
    {
        var histogramValues = GetHistogram(0, Width, 0, Height);

        int ignorePixelsCount = (int)(Width * Height * ignorePart);
        int[] ignoredPixels = new int[3];

        int minColor = 0;
        for (int c = 0; c < histogramValues[0].Length; c++)
        {
            for (int i = 0; i < 3; i++)
            {
                ignoredPixels[i] += (int)histogramValues[i][c];
            }

            if (!ignoredPixels.Any(x => x > ignorePixelsCount)) continue;
            minColor = c;
            break;
        }

        ignoredPixels = new int[3];
        int maxColor = 255;
        for (int c = histogramValues[0].Length - 1; c >= 0; c--)
        {
            for (int i = 0; i < 3; i++)
            {
                ignoredPixels[i] += (int)histogramValues[i][c];
            }

            if (!ignoredPixels.Any(x => x > ignorePixelsCount)) continue;
            maxColor = c;
            break;
        }

        var multCoefficient = 255f / (maxColor - minColor);

        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                var fc = Math.Min(255, Math.Max(0, ((int)Matrix[y, x].FirstComponent - minColor) * multCoefficient));
                var sc = Math.Min(255, Math.Max(0, ((int)Matrix[y, x].SecondComponent - minColor) * multCoefficient));
                var tc = Math.Min(255, Math.Max(0, ((int)Matrix[y, x].ThirdComponent - minColor) * multCoefficient));
                Matrix[y, x] = new Color(fc, sc, tc);
            }
        }
    }

    public override bool Equals(object obj)
    {
        if (obj is not Bitmap bitmap)
        {
            return false;
        }

        const double eps = 1;
        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                if (Math.Abs(Matrix[y, x].FirstComponent - bitmap.Matrix[y, x].FirstComponent) > eps ||
                    Math.Abs(Matrix[y, x].SecondComponent - bitmap.Matrix[y, x].SecondComponent) > eps ||
                    Math.Abs(Matrix[y, x].ThirdComponent - bitmap.Matrix[y, x].ThirdComponent) > eps)
                {
                    return false;
                }
            }
        }

        return true;
    }
}