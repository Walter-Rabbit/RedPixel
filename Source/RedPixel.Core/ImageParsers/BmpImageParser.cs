using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using RedPixel.Core.Colors;
using RedPixel.Core.Colors.Extensions;
using RedPixel.Core.Colors.ValueObjects;
using RedPixel.Core.Tools;

namespace RedPixel.Core.ImageParsers;

public class BmpImageParser : IImageParser
{
    public Core.ImageFormat[] ImageFormats => new[] { Core.ImageFormat.Bmp, };

    public Bitmap.Bitmap Parse(Stream content, ColorSpace colorSpace)
    {
        throw new NotImplementedException();
    }

    public void SerializeToStream(
        Bitmap.Bitmap image,
        Stream stream,
        ColorSpace colorSpace,
        ColorComponents components)
    {
        stream.Write(Encoding.ASCII.GetBytes("BM"));

        int size = 54 + image.Width * image.Height * 3;

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
        {
            for (var x = 0; x < image.Width; x++)
            {
                var pixel = colorSpace.Converter.Invoke(image.GetPixel(x, y).ToRgb(components));
                // TODO: fix
                stream.WriteByte(pixel.ThirdComponent.ToBytes(pixel.BytesForColor)[0]);
                stream.WriteByte(pixel.SecondComponent.ToBytes(pixel.BytesForColor)[0]);
                stream.WriteByte(pixel.FirstComponent.ToBytes(pixel.BytesForColor)[0]);
                stream.WriteByte(1);
            }
        }
    }

    [SuppressMessage("ReSharper.DPA", "DPA0001: Memory allocation issues")]
    public void GetBmpStreamForAvalonia(
        Bitmap.Bitmap image,
        Stream stream,
        ColorSpace colorSpace,
        ColorComponents components,
        float gammaValue)
    {
        stream.Write(Encoding.ASCII.GetBytes("BM"));

        int size = 54 + image.Width * image.Height * 3;

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

        for (int y = image.Height - 1; y >= 0; y--)
        {
            for (int x = 0; x < image.Width; x++)
            {
                var pixel = colorSpace.Converter
                    .Invoke(image.GetPixel(x, y).ToRgb(components));

                // TODO: fix
                stream.WriteByte(pixel.ThirdComponent.ToBytes(pixel.BytesForColor)[0]);
                stream.WriteByte(pixel.SecondComponent.ToBytes(pixel.BytesForColor)[0]);
                stream.WriteByte(pixel.FirstComponent.ToBytes(pixel.BytesForColor)[0]);
                stream.WriteByte(1);
            }
        }
    }
}