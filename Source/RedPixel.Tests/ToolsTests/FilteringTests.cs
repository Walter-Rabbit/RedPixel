using RedPixel.Core.Colors;
using RedPixel.Core.Colors.ValueObjects;
using RedPixel.Core.Models;

namespace RedPixel.Tests.ToolsTests;

public class FilteringTests
{
    private Bitmap _testBitmap;

    [SetUp]
    public void Setup()
    {
        var matrix = new Color[,]
        {
            { new(0, 0, 0), new(100, 100, 100) },
            { new(150, 150, 150), new(255, 255, 255) }
        };
        _testBitmap = new Bitmap(4, 4, 1, ColorSpaces.Rgb)
        {
            Matrix = matrix
        };
    }

    [Theory]
    public void Test1()
    {
    }
}