using System.Drawing;
using RedPixel.Core;
using RedPixel.Core.Colors;
using RedPixel.Core.ImageParsers;
using RedPixel.Core.Tools.Filtering;

namespace RedPixel.Tests.ToolsTests;

public class FilteringTests
{
    [Theory]
    [TestCase("Images\\random-4x4.png", "Images\\FilteringResults\\sobel-random-4x4.png")]
    [TestCase("Images\\random-10x10.png", "Images\\FilteringResults\\sobel-random-10x10.png")]
    [TestCase("Images\\random-15x15.png", "Images\\FilteringResults\\sobel-random-15x15.png")]
    [TestCase("Images\\kittie.png", "Images\\FilteringResults\\sobel-kittie.png")]
    public async Task Sobel(string inputImageName, string outputImageName)
    {
        await using var inputImageStream = File.OpenRead(inputImageName);
        var inputImageFormat = ImageFormat.Parse(inputImageStream);
        var inputImage = ImageParserFactory.CreateParser(inputImageFormat).Parse(inputImageStream, ColorSpaces.Rgb);

        var bitmap = FilteringAlgorithms.Sobel.ApplyFiltering(
            inputImage,
            0,
            new Point(0, 0),
            new Point(inputImage.Width - 1, inputImage.Height - 1));

        await using var outputImageStream = File.OpenRead(outputImageName);
        var outputImageFormat = ImageFormat.Parse(outputImageStream);
        var outputImage = ImageParserFactory.CreateParser(outputImageFormat).Parse(outputImageStream, ColorSpaces.Rgb);

        Assert.That(bitmap, Is.EqualTo(outputImage));
    }
}