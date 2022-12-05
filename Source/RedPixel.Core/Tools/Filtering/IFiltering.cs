using System.Drawing;
using Bitmap = RedPixel.Core.Models.Bitmap;

namespace RedPixel.Core.Tools.Filtering;

public interface IFiltering
{
    static abstract Bitmap ApplyFiltering(Bitmap bitmap, float parameter, Point leftTopPoint, Point rightBottomPoint);
}