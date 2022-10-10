using System.Drawing;
using System.Drawing.Drawing2D;

namespace RedPixel.Core.Bitmap;

public class Bitmap
{
    private readonly List<List<Color>> _matrix;

    public int Width => _matrix.Count == 0 ? 0 : _matrix[0].Count;
    public int Height => _matrix.Count;

    public Bitmap(int width, int height)
    {
        _matrix = new List<List<Color>>(height);
        for (var i = 0; i < _matrix.Count; ++i)
        {
            _matrix[i] = new List<Color>(width);
        }
    }

    public Bitmap(Image image)
        : this(image.Width, image.Height)
    { }

    public Bitmap(Bitmap image)
        : this(image.Width, image.Height)
    { }
    
    public void SetPixel(int x, int y, Color clr)
    {
        ValidateCoordinate(x, y);
        _matrix[y][x] = clr;
    }

    public Color GetPixel(int x, int y) 
    {
        ValidateCoordinate(x, y);
        return _matrix[y][x];
    }

    private void ValidateCoordinate(int x, int y)
    {
        if (y >= Height || x >= Width)
        {
            throw new IndexOutOfRangeException();
        }
    }
}