using RedPixel.Core.Colors.ValueObjects;
using RedPixel.Core.Models;

namespace RedPixel.Core.Colors;

public interface IColorSpace
{
    static abstract Color ToRgb(in Color bitmap, ColorComponents components = ColorComponents.All);
    static abstract Color FromRgb(in Color bitmap);
    static abstract void ToRgb(Bitmap color, ColorComponents components = ColorComponents.All);
    static abstract void FromRgb(Bitmap color);
}