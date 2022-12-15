using System.Drawing;
using Bitmap = RedPixel.Core.Models.Bitmap;
using Color = RedPixel.Core.Colors.ValueObjects.Color;

namespace RedPixel.Core.Tools.Filtering;

public class OtsuFiltering : IFiltering
{
    public static Bitmap ApplyFiltering(Bitmap bitmap, float _, Point leftTopPoint, Point rightBottomPoint)
    {
        var histogram = bitmap.GetHistogram(
            leftTopPoint.X,
            rightBottomPoint.X,
            leftTopPoint.Y,
            rightBottomPoint.Y);
        var thresholds = new float[3];


        for (var i = 0; i < 3; i++)
        {
            var multiSum = 0;
            var sum = 0;
            for (var j = 0; j < 255 * bitmap.BytesForColor; j++)
            {
                multiSum += j * (int)histogram[i][j];
                sum += (int)histogram[i][j];
            }

            var maxSigma = -1.0;
            var class1MultiSum = 0;
            var class1Sum = 0;

            for (var j = 0; j < 255 * bitmap.BytesForColor; j++)
            {
                class1MultiSum += j * (int)histogram[i][j];
                class1Sum += (int)histogram[i][j];

                var class1Probability = (float)class1Sum / sum;
                var middle = (float)class1MultiSum / class1Sum -
                             (float)(multiSum - class1MultiSum) / (sum - class1Sum);

                var sigma = class1Probability * (1 - class1Probability) * middle * middle;

                if (!(sigma > maxSigma)) continue;
                maxSigma = sigma;
                thresholds[i] = j;
            }
        }

        for (var i = leftTopPoint.X; i <= rightBottomPoint.X; i++)
        {
            for (var j = leftTopPoint.Y; j <= rightBottomPoint.Y; j++)
            {
                var fc = bitmap.GetPixel(i, j).FirstComponent > thresholds[0] ? 255 : 0;
                var sc = bitmap.GetPixel(i, j).SecondComponent > thresholds[1] ? 255 : 0;
                var tc = bitmap.GetPixel(i, j).ThirdComponent > thresholds[2] ? 255 : 0;
                bitmap.SetPixel(i, j, new Color(fc, sc, tc));
            }
        }

        return bitmap;
    }
}