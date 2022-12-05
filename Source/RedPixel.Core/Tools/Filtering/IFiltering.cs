using RedPixel.Core.Models;

namespace RedPixel.Core.Tools.Filtering;

public interface IFiltering
{
    static abstract void ApplyFiltering(Bitmap bitmap, float parameter);
}