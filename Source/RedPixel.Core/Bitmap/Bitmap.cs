using System.Drawing;

namespace RedPixel.Core.Bitmap;

public class Bitmap
{
    private readonly Color[,] _matrix;

    public int Width => _matrix.Length == 0 ? 0 : _matrix.GetLength(1);
    public int Height => _matrix.GetLength(0);

    public Bitmap(int width, int height)
    {
        _matrix = new Color[height, width];
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
    
    public void SetPixel(int x, int y, Color clr)
    {
        ValidateCoordinate(x, y);
        _matrix[y, x] = clr;
    }

    public Image GetSystemBitmap()
    {
        System.Drawing.Bitmap img = new System.Drawing.Bitmap(Width, Height);
        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                img.SetPixel(x, y, GetPixel(x, y));
            }
        }

        return img;
    }

    public Color GetPixel(int x, int y) 
    {
        ValidateCoordinate(x, y);
        return _matrix[y, x];
    }

    private void ValidateCoordinate(int x, int y)
    {
        if (y >= Height || x >= Width)
        {
            throw new IndexOutOfRangeException();
        }
    }
}