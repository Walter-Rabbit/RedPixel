using System.Drawing;
using Bitmap = RedPixel.Core.Models.Bitmap;
using Color = RedPixel.Core.Colors.ValueObjects.Color;

namespace RedPixel.Core.Tools.Filtering;

public class CasFiltering : IFiltering
{
    public static Bitmap ApplyFiltering(Bitmap bitmap, float sharpness, Point leftTopPoint, Point rightBottomPoint)
    {
        var areaPixels = new List<float[]>
        {
            new float[4],
            new float[4],
            new float[4],
        };
        var newBitmap = new Bitmap(bitmap.Width, bitmap.Height, bitmap.BytesForColor, bitmap.ColorSpace);
        newBitmap.Matrix = bitmap.Matrix.Clone() as Color[,];

        for (var i = leftTopPoint.X; i <= rightBottomPoint.X; i++)
        {
            for (var j = leftTopPoint.Y; j <= rightBottomPoint.Y; j++)
            {
                GetAreaPixels(bitmap, i, j, areaPixels, leftTopPoint, rightBottomPoint);

                var fc = areaPixels[0].Sum() / 4 * (1 - sharpness) + sharpness * bitmap.GetPixel(i, j).FirstComponent;
                var sc = areaPixels[1].Sum() / 4 * (1 - sharpness) + sharpness * bitmap.GetPixel(i, j).SecondComponent;
                var tc = areaPixels[2].Sum() / 4 * (1 - sharpness) + sharpness * bitmap.GetPixel(i, j).ThirdComponent;

                newBitmap.SetPixel(i, j, new Color(fc, sc, tc));
            }
        }

        return newBitmap;
    }

    private static void GetAreaPixels(
        Bitmap bitmap,
        int x,
        int y,
        List<float[]> areaPixels,
        Point leftTopPoint,
        Point rightBottomPoint)
    {
        areaPixels[0][0] = IFiltering.GetClosestPixel(bitmap, x, y + 1, leftTopPoint, rightBottomPoint).FirstComponent;
        areaPixels[1][0] = IFiltering.GetClosestPixel(bitmap, x, y + 1, leftTopPoint, rightBottomPoint).SecondComponent;
        areaPixels[2][0] = IFiltering.GetClosestPixel(bitmap, x, y + 1, leftTopPoint, rightBottomPoint).ThirdComponent;

        areaPixels[0][1] = IFiltering.GetClosestPixel(bitmap, x, y - 1, leftTopPoint, rightBottomPoint).FirstComponent;
        areaPixels[1][1] = IFiltering.GetClosestPixel(bitmap, x, y - 1, leftTopPoint, rightBottomPoint).SecondComponent;
        areaPixels[2][1] = IFiltering.GetClosestPixel(bitmap, x, y - 1, leftTopPoint, rightBottomPoint).ThirdComponent;

        areaPixels[0][2] = IFiltering.GetClosestPixel(bitmap, x + 1, y, leftTopPoint, rightBottomPoint).FirstComponent;
        areaPixels[1][2] = IFiltering.GetClosestPixel(bitmap, x + 1, y, leftTopPoint, rightBottomPoint).SecondComponent;
        areaPixels[2][2] = IFiltering.GetClosestPixel(bitmap, x + 1, y, leftTopPoint, rightBottomPoint).ThirdComponent;

        areaPixels[0][3] = IFiltering.GetClosestPixel(bitmap, x - 1, y, leftTopPoint, rightBottomPoint).FirstComponent;
        areaPixels[1][3] = IFiltering.GetClosestPixel(bitmap, x - 1, y, leftTopPoint, rightBottomPoint).SecondComponent;
        areaPixels[2][3] = IFiltering.GetClosestPixel(bitmap, x - 1, y, leftTopPoint, rightBottomPoint).ThirdComponent;
    }
}