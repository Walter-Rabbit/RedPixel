using System.Drawing;
using Bitmap = RedPixel.Core.Models.Bitmap;
using Color = RedPixel.Core.Colors.ValueObjects.Color;

namespace RedPixel.Core.Tools.Filtering;

public class CasFiltering : IFiltering
{
    public static Bitmap ApplyFiltering(Bitmap bitmap, float sharpness, Point leftTopPoint, Point rightBottomPoint)
    {
        var areaPixels = new float[3, 4]
        {
            { 0, 0, 0, 0 },
            { 0, 0, 0, 0 },
            { 0, 0, 0, 0 },
        };
        var newBitmap = new Bitmap(bitmap.Width, bitmap.Height, bitmap.BytesForColor, bitmap.ColorSpace);
        newBitmap.Matrix = bitmap.Matrix.Clone() as Color[,];

        for (var i = leftTopPoint.X; i <= rightBottomPoint.X; i++)
        {
            for (var j = leftTopPoint.Y; j <= rightBottomPoint.Y; j++)
            {
                GetAreaPixels(bitmap, i, j, areaPixels, leftTopPoint, rightBottomPoint);

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

                var fc = (Sum(areaPixels, 0) * fcWeight + bitmap.GetPixel(i, j).FirstComponent) / (fcWeight * 4f + 1f);
                var sc = (Sum(areaPixels, 1) * scWeight + bitmap.GetPixel(i, j).SecondComponent) / (scWeight * 4f + 1f);
                var tc = (Sum(areaPixels, 2) * tcWeight + bitmap.GetPixel(i, j).ThirdComponent) / (tcWeight * 4f + 1f);

                newBitmap.SetPixel(i, j, new Color(
                    Math.Max(0f, Math.Min(255f, fc)),
                    Math.Max(0f, Math.Min(255f, sc)),
                    Math.Max(0f, Math.Min(255f, tc))));
            }
        }

        return newBitmap;
    }

    private static void GetAreaPixels(
        Bitmap bitmap,
        int x,
        int y,
        float[,] areaPixels,
        Point leftTopPoint,
        Point rightBottomPoint)
    {
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
    }

    private static float Sum(float[,] matrix, int row)
    {
        var sum = 0f;
        for (var i = 0; i < matrix.GetLength(1); i++)
        {
            sum += matrix[row, i];
        }

        return sum;
    }
}