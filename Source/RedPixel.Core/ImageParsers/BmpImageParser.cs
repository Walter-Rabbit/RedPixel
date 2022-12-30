using System.Text;
using RedPixel.Core.Colors;
using RedPixel.Core.Colors.ValueObjects;
using RedPixel.Core.Models;
using RedPixel.Core.Tools;

namespace RedPixel.Core.ImageParsers;

/// <summary>
///     Только для последующей конвертации в avalonia bitmap. Не использовать для сохранения файла.
/// </summary>
public class BmpImageParser : IImageParser
{
    public ImageFormat[] ImageFormats => new[] { ImageFormat.Bmp };

    public Bitmap Parse(Stream content, ColorSpaces colorSpaces)
    {
        throw new NotImplementedException();
    }

    public void SerializeToStream(Bitmap image, Stream stream, ColorSpaces colorSpace, ColorComponents components)
    {
        stream.Write(Encoding.ASCII.GetBytes("BM"));

        var size = 54 + image.Width * image.Height * 3;

        // TODO: Use Span<byte> to avoid allocations
        stream.Write(BitConverter.GetBytes(size), 0, 4);
        stream.Write(BitConverter.GetBytes(0), 0, 4);
        stream.Write(BitConverter.GetBytes(54), 0, 4);
        stream.Write(BitConverter.GetBytes(40), 0, 4);
        stream.Write(BitConverter.GetBytes(image.Width), 0, 4);
        stream.Write(BitConverter.GetBytes(image.Height), 0, 4);
        stream.Write(BitConverter.GetBytes((short)1), 0, 2);
        stream.Write(BitConverter.GetBytes((short)32), 0, 2);
        stream.Write(BitConverter.GetBytes(0), 0, 4);
        stream.Write(BitConverter.GetBytes(0), 0, 4);
        stream.Write(BitConverter.GetBytes(0), 0, 4);
        stream.Write(BitConverter.GetBytes(0), 0, 4);
        stream.Write(BitConverter.GetBytes(256 * 256 * 256), 0, 4);
        stream.Write(BitConverter.GetBytes(0), 0, 4);

        for (var y = image.Height - 1; y >= 0; y--)
        for (var x = 0; x < image.Width; x++)
        {
            var pixel = image.Matrix[y, x];
            if (colorSpace != image.ColorSpace || components != ColorComponents.All)
            {
                pixel = image.ColorSpace.ColorToRgb(in pixel, components);
                pixel = colorSpace.ColorFromRgb(in pixel);
            }

            pixel = pixel.AssignGamma(image.Gamma);
            // TODO: fix
            stream.WriteByte(pixel.ThirdComponent.ToBytes(image.BytesForColor)[0]);
            stream.WriteByte(pixel.SecondComponent.ToBytes(image.BytesForColor)[0]);
            stream.WriteByte(pixel.FirstComponent.ToBytes(image.BytesForColor)[0]);
            stream.WriteByte(1);
        }
    }
}