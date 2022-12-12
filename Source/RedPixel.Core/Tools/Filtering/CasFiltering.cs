using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using Bitmap = RedPixel.Core.Models.Bitmap;
using Color = RedPixel.Core.Colors.ValueObjects.Color;

namespace RedPixel.Core.Tools.Filtering;

public class CasFiltering : IFiltering
{
    [SuppressMessage("ReSharper.DPA", "DPA0001: Memory allocation issues")]
    public static Bitmap ApplyFiltering(Bitmap bitmap, float sharpness, Point leftTopPoint, Point rightBottomPoint)
    {
        var newBitmap = new Bitmap(bitmap.Width, bitmap.Height, bitmap.BytesForColor, bitmap.ColorSpace)
        {
            Matrix = bitmap.Matrix.Clone() as Color[,]
        };

        var width = rightBottomPoint.X - leftTopPoint.X + 1f;
        var height = rightBottomPoint.Y - leftTopPoint.Y + 1f;

        for (var i = leftTopPoint.X; i <= rightBottomPoint.X; i++)
        {
            for (var j = leftTopPoint.Y; j <= rightBottomPoint.Y; j++)
            {
                var (ltWeights, ltMin, ltMax) =
                    GetPixelWeights(i, j, bitmap, sharpness, leftTopPoint, rightBottomPoint);
                var (rtWeights, rtMin, rtMax) =
                    GetPixelWeights(i + 1, j, bitmap, sharpness, leftTopPoint, rightBottomPoint);
                var (lbWeights, lbMin, lbMax) =
                    GetPixelWeights(i, j + 1, bitmap, sharpness, leftTopPoint, rightBottomPoint);
                var (rbWeights, rbMin, rbMax) =
                    GetPixelWeights(i + 1, j + 1, bitmap, sharpness, leftTopPoint, rightBottomPoint);

                var fractionalPositionX = i / width;
                var fractionalPositionY = j / height;
                var ltCoefficient = (1f - fractionalPositionX) * (1f - fractionalPositionY);
                var rtCoefficient = fractionalPositionX * (1f - fractionalPositionY);
                var lbCoefficient = (1f - fractionalPositionX) * fractionalPositionY;
                var rbCoefficient = fractionalPositionX * fractionalPositionY;

                var lt = IFiltering.GetClosestPixel(bitmap, i, j, leftTopPoint, rightBottomPoint);
                var llt = IFiltering.GetClosestPixel(bitmap, i - 1, j, leftTopPoint, rightBottomPoint);
                var tlt = IFiltering.GetClosestPixel(bitmap, i, j - 1, leftTopPoint, rightBottomPoint);

                var rt = IFiltering.GetClosestPixel(bitmap, i + 1, j, leftTopPoint, rightBottomPoint);
                var trt = IFiltering.GetClosestPixel(bitmap, i + 1, j - 1, leftTopPoint, rightBottomPoint);
                var rrt = IFiltering.GetClosestPixel(bitmap, i + 2, j, leftTopPoint, rightBottomPoint);

                var rb = IFiltering.GetClosestPixel(bitmap, i + 1, j + 1, leftTopPoint, rightBottomPoint);
                var rrb = IFiltering.GetClosestPixel(bitmap, i + 2, j + 1, leftTopPoint, rightBottomPoint);
                var brb = IFiltering.GetClosestPixel(bitmap, i + 1, j + 2, leftTopPoint, rightBottomPoint);

                var lb = IFiltering.GetClosestPixel(bitmap, i, j + 1, leftTopPoint, rightBottomPoint);
                var blb = IFiltering.GetClosestPixel(bitmap, i, j + 2, leftTopPoint, rightBottomPoint);
                var llb = IFiltering.GetClosestPixel(bitmap, i - 1, j + 1, leftTopPoint, rightBottomPoint);

                var ltCoefficientFinal = ltCoefficient / (1f / 32f + (ltMax[0] - ltMin[0]));
                var rtCoefficientFinal = rtCoefficient / (1f / 32f + (rtMax[0] - rtMin[0]));
                var rbCoefficientFinal = rbCoefficient / (1f / 32f + (rbMax[0] - rbMin[0]));
                var lbCoefficientFinal = lbCoefficient / (1f / 32f + (lbMax[0] - lbMin[0]));

                var ltWeightsFinal = ltWeights.Select(w => w * ltCoefficientFinal).ToArray();
                var rtWeightsFinal = rtWeights.Select(w => w * rtCoefficientFinal).ToArray();
                var rbWeightsFinal = rbWeights.Select(w => w * rbCoefficientFinal).ToArray();
                var lbWeightsFinal = lbWeights.Select(w => w * lbCoefficientFinal).ToArray();

                var fc = (ltWeightsFinal[0] * tlt.FirstComponent +
                          rtWeightsFinal[0] * trt.FirstComponent +
                          rtWeightsFinal[0] * rrt.FirstComponent +
                          rbWeightsFinal[0] * rrb.FirstComponent +
                          rbWeightsFinal[0] * brb.FirstComponent +
                          lbWeightsFinal[0] * blb.FirstComponent +
                          lbWeightsFinal[0] * llb.FirstComponent +
                          ltWeightsFinal[0] * llt.FirstComponent +
                          (rtWeightsFinal[0] + lbWeightsFinal[0] + ltCoefficientFinal) * lt.FirstComponent +
                          (ltWeightsFinal[0] + rbWeightsFinal[0] + rtCoefficientFinal) * rt.FirstComponent +
                          (rtWeightsFinal[0] + lbWeightsFinal[0] + rbCoefficientFinal) * rb.FirstComponent +
                          (ltWeightsFinal[0] + rbWeightsFinal[0] + lbCoefficientFinal) * lb.FirstComponent) /
                         (2f * (ltWeightsFinal[0] + rtWeightsFinal[0] + rbWeightsFinal[0] + lbWeightsFinal[0]) +
                          (rtWeightsFinal[0] + lbWeightsFinal[0] + ltCoefficientFinal) +
                          (ltWeightsFinal[0] + rbWeightsFinal[0] + rtCoefficientFinal) +
                          (rtWeightsFinal[0] + lbWeightsFinal[0] + rbCoefficientFinal) +
                          (ltWeightsFinal[0] + rbWeightsFinal[0] + lbCoefficientFinal));


                ltCoefficientFinal = ltCoefficient / (1f / 32f + (ltMax[1] - ltMin[1]));
                rtCoefficientFinal = rtCoefficient / (1f / 32f + (rtMax[1] - rtMin[1]));
                rbCoefficientFinal = rbCoefficient / (1f / 32f + (rbMax[1] - rbMin[1]));
                lbCoefficientFinal = lbCoefficient / (1f / 32f + (lbMax[1] - lbMin[1]));

                ltWeightsFinal = ltWeights.Select(w => w * ltCoefficientFinal).ToArray();
                rtWeightsFinal = rtWeights.Select(w => w * rtCoefficientFinal).ToArray();
                rbWeightsFinal = rbWeights.Select(w => w * rbCoefficientFinal).ToArray();
                lbWeightsFinal = lbWeights.Select(w => w * lbCoefficientFinal).ToArray();

                var sc = (ltWeightsFinal[1] * tlt.SecondComponent +
                          rtWeightsFinal[1] * trt.SecondComponent +
                          rtWeightsFinal[1] * rrt.SecondComponent +
                          rbWeightsFinal[1] * rrb.SecondComponent +
                          rbWeightsFinal[1] * brb.SecondComponent +
                          lbWeightsFinal[1] * blb.SecondComponent +
                          lbWeightsFinal[1] * llb.SecondComponent +
                          ltWeightsFinal[1] * llt.SecondComponent +
                          (rtWeightsFinal[1] + lbWeightsFinal[1] + ltCoefficientFinal) * lt.SecondComponent +
                          (ltWeightsFinal[1] + rbWeightsFinal[1] + rtCoefficientFinal) * rt.SecondComponent +
                          (rtWeightsFinal[1] + lbWeightsFinal[1] + rbCoefficientFinal) * rb.SecondComponent +
                          (ltWeightsFinal[1] + rbWeightsFinal[1] + lbCoefficientFinal) * lb.SecondComponent) /
                         (2f * (ltWeightsFinal[1] + rtWeightsFinal[1] + rbWeightsFinal[1] + lbWeightsFinal[1]) +
                          (rtWeightsFinal[1] + lbWeightsFinal[1] + ltCoefficientFinal) +
                          (ltWeightsFinal[1] + rbWeightsFinal[1] + rtCoefficientFinal) +
                          (rtWeightsFinal[1] + lbWeightsFinal[1] + rbCoefficientFinal) +
                          (ltWeightsFinal[1] + rbWeightsFinal[1] + lbCoefficientFinal));


                ltCoefficientFinal = ltCoefficient / (1f / 32f + (ltMax[2] - ltMin[2]));
                rtCoefficientFinal = rtCoefficient / (1f / 32f + (rtMax[2] - rtMin[2]));
                rbCoefficientFinal = rbCoefficient / (1f / 32f + (rbMax[2] - rbMin[2]));
                lbCoefficientFinal = lbCoefficient / (1f / 32f + (lbMax[2] - lbMin[2]));

                ltWeightsFinal = ltWeights.Select(w => w * ltCoefficientFinal).ToArray();
                rtWeightsFinal = rtWeights.Select(w => w * rtCoefficientFinal).ToArray();
                rbWeightsFinal = rbWeights.Select(w => w * rbCoefficientFinal).ToArray();
                lbWeightsFinal = lbWeights.Select(w => w * lbCoefficientFinal).ToArray();

                var tc = (ltWeightsFinal[2] * tlt.ThirdComponent +
                          rtWeightsFinal[2] * trt.ThirdComponent +
                          rtWeightsFinal[2] * rrt.ThirdComponent +
                          rbWeightsFinal[2] * rrb.ThirdComponent +
                          rbWeightsFinal[2] * brb.ThirdComponent +
                          lbWeightsFinal[2] * blb.ThirdComponent +
                          lbWeightsFinal[2] * llb.ThirdComponent +
                          ltWeightsFinal[2] * llt.ThirdComponent +
                          (rtWeightsFinal[2] + lbWeightsFinal[2] + ltCoefficientFinal) * lt.ThirdComponent +
                          (ltWeightsFinal[2] + rbWeightsFinal[2] + rtCoefficientFinal) * rt.ThirdComponent +
                          (rtWeightsFinal[2] + lbWeightsFinal[2] + rbCoefficientFinal) * rb.ThirdComponent +
                          (ltWeightsFinal[2] + rbWeightsFinal[2] + lbCoefficientFinal) * lb.ThirdComponent) /
                         (2f * (ltWeightsFinal[2] + rtWeightsFinal[2] + rbWeightsFinal[2] + lbWeightsFinal[2]) +
                          (rtWeightsFinal[2] + lbWeightsFinal[2] + ltCoefficientFinal) +
                          (ltWeightsFinal[2] + rbWeightsFinal[2] + rtCoefficientFinal) +
                          (rtWeightsFinal[2] + lbWeightsFinal[2] + rbCoefficientFinal) +
                          (ltWeightsFinal[2] + rbWeightsFinal[2] + lbCoefficientFinal));

                newBitmap.SetPixel(i, j, new Color(
                    Math.Max(0f, Math.Min(255f, fc)),
                    Math.Max(0f, Math.Min(255f, sc)),
                    Math.Max(0f, Math.Min(255f, tc))));
            }
        }

        return newBitmap;
    }

    private static (float[], float[], float[]) GetPixelWeights(int i, int j, Bitmap bitmap, float sharpness,
        Point leftTopPoint, Point rightBottomPoint)
    {
        var areaPixels = GetAreaPixels(bitmap, i, j, leftTopPoint, rightBottomPoint);

        var fcMin = areaPixels[0, 0];
        var fcMax = areaPixels[0, 0];
        var scMin = areaPixels[1, 0];
        var scMax = areaPixels[1, 0];
        var tcMin = areaPixels[2, 0];
        var tcMax = areaPixels[2, 0];

        for (var k = 0; k < 4; k++)
        {
            fcMax = Math.Max(areaPixels[0, k], fcMax);
            fcMin = Math.Min(areaPixels[0, k], fcMin);
            scMax = Math.Max(areaPixels[1, k], scMax);
            scMin = Math.Min(areaPixels[1, k], scMin);
            tcMax = Math.Max(areaPixels[2, k], tcMax);
            tcMin = Math.Min(areaPixels[2, k], tcMin);
        }

        var fcMaxDiff = 255f - fcMax;
        var scMaxDiff = 255f - scMax;
        var tcMaxDiff = 255f - tcMax;

        var fcCoefficient = (float)Math.Sqrt(Math.Min(fcMin, fcMaxDiff) / fcMax);
        var scCoefficient = (float)Math.Sqrt(Math.Min(scMin, scMaxDiff) / scMax);
        var tcCoefficient = (float)Math.Sqrt(Math.Min(tcMin, tcMaxDiff) / tcMax);

        var sharpnessMax = -0.125f * (1f - sharpness) + -0.2f * sharpness;
        var fcWeight = fcCoefficient * sharpnessMax;
        var scWeight = scCoefficient * sharpnessMax;
        var tcWeight = tcCoefficient * sharpnessMax;

        return (new float[]
                {
                    fcWeight, scWeight, tcWeight
                },
                new float[]
                {
                    fcMin / 255f,
                    scMin / 255f,
                    tcMin / 255f,
                },
                new float[]
                {
                    fcMax / 255f,
                    scMax / 255f,
                    tcMax / 255f,
                }
            );
    }

    private static float[,] GetAreaPixels(
        Bitmap bitmap,
        int x,
        int y,
        Point leftTopPoint,
        Point rightBottomPoint)
    {
        var areaPixels = new float[3, 4]
        {
            { 0, 0, 0, 0 },
            { 0, 0, 0, 0 },
            { 0, 0, 0, 0 },
        };

        areaPixels[0, 0] = IFiltering.GetClosestPixel(bitmap, x, y + 1, leftTopPoint, rightBottomPoint).FirstComponent;
        areaPixels[1, 0] = IFiltering.GetClosestPixel(bitmap, x, y + 1, leftTopPoint, rightBottomPoint).SecondComponent;
        areaPixels[2, 0] = IFiltering.GetClosestPixel(bitmap, x, y + 1, leftTopPoint, rightBottomPoint).ThirdComponent;

        areaPixels[0, 1] = IFiltering.GetClosestPixel(bitmap, x, y - 1, leftTopPoint, rightBottomPoint).FirstComponent;
        areaPixels[1, 1] = IFiltering.GetClosestPixel(bitmap, x, y - 1, leftTopPoint, rightBottomPoint).SecondComponent;
        areaPixels[2, 1] = IFiltering.GetClosestPixel(bitmap, x, y - 1, leftTopPoint, rightBottomPoint).ThirdComponent;

        areaPixels[0, 2] = IFiltering.GetClosestPixel(bitmap, x + 1, y, leftTopPoint, rightBottomPoint).FirstComponent;
        areaPixels[1, 2] = IFiltering.GetClosestPixel(bitmap, x + 1, y, leftTopPoint, rightBottomPoint).SecondComponent;
        areaPixels[2, 2] = IFiltering.GetClosestPixel(bitmap, x + 1, y, leftTopPoint, rightBottomPoint).ThirdComponent;

        areaPixels[0, 3] = IFiltering.GetClosestPixel(bitmap, x - 1, y, leftTopPoint, rightBottomPoint).FirstComponent;
        areaPixels[1, 3] = IFiltering.GetClosestPixel(bitmap, x - 1, y, leftTopPoint, rightBottomPoint).SecondComponent;
        areaPixels[2, 3] = IFiltering.GetClosestPixel(bitmap, x - 1, y, leftTopPoint, rightBottomPoint).ThirdComponent;

        return areaPixels;
    }
}