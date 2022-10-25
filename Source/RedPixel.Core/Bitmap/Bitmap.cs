using System.Drawing;
using RedPixel.Core.Colors;

namespace RedPixel.Core.Bitmap;

public class Bitmap
{
    private readonly IColor[,] _matrix;

    public int Width => _matrix.Length == 0 ? 0 : _matrix.GetLength(1);
    public int Height => _matrix.GetLength(0);

    public Bitmap(int width, int height)
    {
        _matrix = new IColor[height, width];
    }

    public Bitmap(Image image)
        : this(image.Width, image.Height)
    { }

    public Bitmap(Bitmap image)
        : this(image.Width, image.Height)
    {
        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                SetPixel(x, y, image.GetPixel(x, y));
            }
        }
    }

    public void SetPixel(int x, int y, IColor clr)
    {
        _matrix[y, x] = clr;
    }

    public IColor GetPixel(int x, int y)
    {
        return _matrix[y, x];
    }

    public Bitmap SelectColorComponents(ColorComponents component)
    {
        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                _matrix[y, x].SelectComponents(component);
            }
        }

        return this;
    }

    public Bitmap ChangeColorSpace(ColorSpace space)
    {
        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                _matrix[y, x] = space.Converter.Invoke(_matrix[y, x]);
            }
        }

        return this;
    }
}