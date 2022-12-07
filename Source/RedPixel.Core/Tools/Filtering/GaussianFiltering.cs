using System.Drawing;
using Bitmap = RedPixel.Core.Models.Bitmap;

namespace RedPixel.Core.Tools.Filtering;

public class GaussianFiltering : IFiltering
{
    public static Bitmap ApplyFiltering(Bitmap bitmap, float sigma, Point leftTopPoint, Point rightBottomPoint)
    {
        var kernel = CalculateGaussianKernel(sigma);

        return IFiltering.Convolution(bitmap, kernel);
    }

    private static float[,] CalculateGaussianKernel(float sigma)
    {
        var coreRadius = (int)Math.Round(3 * sigma);
        var kernelWidth = 2 * coreRadius + 1;

        var kernel = new float[kernelWidth, kernelWidth];
        var kernelSum = 0f;

        for (var i = -coreRadius; i <= coreRadius; i++)
        {
            for (var j = -coreRadius; j <= coreRadius; j++)
            {
                var exponentNumerator = -(i * i + j * j);
                var exponentDenominator = 2 * sigma * sigma;

                var eExpression = Math.Pow(Math.E, exponentNumerator / exponentDenominator);
                var value = (float)(eExpression / (2 * Math.PI * sigma * sigma));

                kernel[i + coreRadius, j + coreRadius] = value;
                kernelSum += value;
            }
        }

        for (var i = 0; i < kernelWidth; i++)
        {
            for (var j = 0; j < kernelWidth; j++)
            {
                kernel[i, j] /= kernelSum;
            }
        }

        return kernel;
    }
}