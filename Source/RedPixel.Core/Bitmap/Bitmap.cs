using System.Diagnostics;
using System.Drawing;
using RedPixel.Core.Colors;
using Color = RedPixel.Core.Colors.Color;

namespace RedPixel.Core.Bitmap;

public class Bitmap
{
    private readonly RgbColor[,] _matrix;

    public int Width => _matrix.Length == 0 ? 0 : _matrix.GetLength(1);
    public int Height => _matrix.GetLength(0);

    public Bitmap(int width, int height)
    {
        _matrix = new RgbColor[height, width];
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

    public void SetPixel(int x, int y, RgbColor clr)
    {
        _matrix[y, x] = clr;
    }

    public RgbColor GetPixel(int x, int y)
    {
        return _matrix[y, x];
    }

    public Bitmap SelectColorComponents(ColorComponents component)
    {
        var sw = new Stopwatch();
        sw.Start();
        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                _matrix[y, x].SelectComponents(component);
            }
        }

        sw.Stop();

        File.AppendAllText("time-log.txt", $"Change components to {component} {sw.ElapsedMilliseconds}ms\r\n");

        return this;
    }
}