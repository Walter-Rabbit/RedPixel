using RedPixel.Core.Colors;
using RedPixel.Core.Colors.Extensions;
using RedPixel.Core.Colors.ValueObjects;
using Color = RedPixel.Core.Colors.ValueObjects.Color;

namespace RedPixel.Core.Models;

public class Bitmap
{
    public Color[,] Matrix { get; }

    public ColorSpaces ColorSpace { get; private set; }

    public int Width => Matrix.Length == 0 ? 0 : Matrix.GetLength(1);
    public int Height => Matrix.GetLength(0);
    public int BytesForColor { get; set; }

    public float Gamma { get; set; } = 0;

    public Bitmap(int width, int height, int bytesForColor, ColorSpaces colorSpace)
    {
        ColorSpace = colorSpace;
        BytesForColor = bytesForColor;
        Matrix = new Color[height, width];
    }

    public void SetPixel(int x, int y, Color clr)
    {
        Matrix[y, x] = clr;
    }

    public Color GetPixel(int x, int y)
    {
        return Matrix[y, x];
    }

    public void ToColorSpace(ColorSpaces colorSpace)
    {
        if (colorSpace == ColorSpace)
            return;

        ColorSpace.BitmapToRgb(this, ColorComponents.All);
        colorSpace.BitmapFromRgb(this);

        ColorSpace = colorSpace;
    }

    public Bitmap AssignGamma(float targetGammaValue)
    {
        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                Matrix[y, x] = Matrix[y, x].AssignGamma(BytesForColor, Gamma, targetGammaValue);
            }
        }

        Gamma = targetGammaValue;
        return this;
    }

    public Bitmap ConvertToGamma(float targetGammaValue)
    {
        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                Matrix[y, x] = Matrix[y, x]
                    .ConvertToGamma(BytesForColor, Gamma, targetGammaValue)
                    .AssignGamma(BytesForColor, Gamma, targetGammaValue);
            }
        }

        Gamma = targetGammaValue;
        return this;
    }
}