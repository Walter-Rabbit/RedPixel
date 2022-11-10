using RedPixel.Core.Colors.ValueObjects;
using RedPixel.Core.Models;

namespace RedPixel.Core.Colors;

public interface IColorSpace
{
    static abstract void ToRgb(ref Color bitmap, ColorComponents components = ColorComponents.All);
    static abstract void FromRgb(ref Color bitmap);
    static abstract void ToRgb(Bitmap bitmap, ColorComponents components = ColorComponents.All);
    static abstract void FromRgb(Bitmap bitmap);
}