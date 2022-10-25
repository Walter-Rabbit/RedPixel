using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using RedPixel.Core;
using RedPixel.Core.Bitmap;
using RedPixel.Core.Colors;
using RedPixel.Core.ImageParsers;
using RedPixel.Ui.Utility;

namespace RedPixel.Benchmark;

public class FileOpenBenchmark
{
    private Bitmap image;

    public FileOpenBenchmark()
    {
        using var fs = File.OpenRead(@"C:\Users\alex8\Desktop\1.pnm");

        var parser = new PnmImageParser();
        image = parser.Parse(fs, ColorSpace.Rgb);
    }
    //
    // [Benchmark]
    // public void OpenFileBenchmark()
    // {
    //     string filePath = @"C:\Users\alex8\Desktop\1.pnm";
    //
    //     using var fs = File.OpenRead(filePath);
    //
    //     var parser = new PnmImageParser();
    //     var image = parser.Parse(fs);
    // }

    [Benchmark]
    public void SingleDimArray()
    {
        var array = new float[10000];

        for (int y = 0; y < 100; y++)
        {
            for (int x = 0; x < 100; x++)
            {
                array[y * 100 + x] = 1;
            }
        }
    }

    [Benchmark]
    public void MultiDimArray()
    {
        var array = new float[100, 100];

        for (int y = 0; y < 100; y++)
        {
            for (int x = 0; x < 100; x++)
            {
                array[y, x] = 1;
            }
        }
    }
}