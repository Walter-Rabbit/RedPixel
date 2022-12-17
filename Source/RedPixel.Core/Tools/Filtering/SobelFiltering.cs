using System.Drawing;
using Bitmap = RedPixel.Core.Models.Bitmap;
using Color = RedPixel.Core.Colors.ValueObjects.Color;

namespace RedPixel.Core.Tools.Filtering;

public class SobelFiltering : IFiltering
{
    public static Bitmap ApplyFiltering(Bitmap bitmap, float _, Point leftTopPoint, Point rightBottomPoint)
    {
        var horizontalKernel = new float[,]
        {
            { 1f, 2f, 1f },
            { 0f, 0f, 0f },
            { -1f, -2f, -1f }
        };
        var verticalKernel = new float[,]
        {
            { -1f, 0f, 1f },
            { -2f, 0f, 2f },
            { -1f, 0f, 1f }
        };

        var horizontalBitmap = IFiltering.Convolution(bitmap, horizontalKernel, leftTopPoint, rightBottomPoint);
        var verticalBitmap = IFiltering.Convolution(bitmap, verticalKernel, leftTopPoint, rightBottomPoint);
        var newBitmap = new Bitmap(bitmap.Width, bitmap.Height, bitmap.BytesForColor, bitmap.ColorSpace)
        {
            Matrix = bitmap.Matrix.Clone() as Color[,]
        };

        for (var x = leftTopPoint.X; x < rightBottomPoint.X; x++)
        {
            for (var y = leftTopPoint.Y; y < rightBottomPoint.Y; y++)
            {
                var fc = (float) Math.Sqrt(
                    Math.Pow(horizontalBitmap.GetPixel(x, y).FirstComponent, 2) +
                    Math.Pow(verticalBitmap.GetPixel(x, y).FirstComponent, 2));

                var sc = (float) Math.Sqrt(
                    Math.Pow(horizontalBitmap.GetPixel(x, y).FirstComponent, 2) +
                    Math.Pow(verticalBitmap.GetPixel(x, y).FirstComponent, 2));

                var tc = (float) Math.Sqrt(
                    Math.Pow(horizontalBitmap.GetPixel(x, y).FirstComponent, 2) +
                    Math.Pow(verticalBitmap.GetPixel(x, y).FirstComponent, 2));

                var pixel = new Color(
                    Math.Max(0, Math.Min(255f, fc)),
                    Math.Max(0, Math.Min(255f, sc)),
                    Math.Max(0, Math.Min(255f, tc)));
                newBitmap.SetPixel(x, y, pixel);
            }
        }

        return newBitmap;
    }
}