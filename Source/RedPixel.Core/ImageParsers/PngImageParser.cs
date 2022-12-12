using RedPixel.Core.Colors;
using RedPixel.Core.Colors.ValueObjects;
using RedPixel.Core.ImageParsers.Chunks;
using RedPixel.Core.Models;
using RedPixelBitmap = RedPixel.Core.Models.Bitmap;

namespace RedPixel.Core.ImageParsers;

public class PngImageParser : IImageParser
{
    private string[] _criticalChunks = { "IHDR", "PLTE", "IDAT", "IEND" };
    
    private List<PngChunk> _chunks;
    
    public ImageFormat[] ImageFormats => new[] { ImageFormat.Png };
    public RedPixelBitmap Parse(Stream content, ColorSpaces colorSpaces)
    {
        _chunks = new List<PngChunk>();
        if (!ImageFormat.Png.IsMatch(content))
        {
            throw new NotSupportedException($"Unsupported image format - {content}");
        }
        content.Read(new byte[8]);
        
        _chunks.Add(new PngChunk(content));
        while (_chunks.Last().Name != "IEND")
        {
            _chunks.Add(new PngChunk(content));
        }

        if (!_criticalChunks.All(it => _chunks.Exists(chunk => chunk.Name == it)))
        {
            throw new Exception("");
        }

        return new RedPixelBitmap(0,0,0, ColorSpaces.Rgb);
    }

    public void SerializeToStream(Bitmap image, Stream stream, ColorSpaces colorSpaces, ColorComponents components)
    {
        throw new NotImplementedException();
    }
}