using RedPixel.Core.Colors.ValueObjects;
using RedPixel.Core.Models;

namespace RedPixel.Core.Tools.Dithering;

public interface IDitheringAlgo
{
    static abstract void ApplyDithering(Bitmap bitmap, ColorDepth depth);
}