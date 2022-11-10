using System.Text;
using RedPixel.Core.Colors;
using RedPixel.Core.Colors.ValueObjects;
using RedPixel.Core.Tools;
using RedPixelBitmap = RedPixel.Core.Models.Bitmap;

namespace RedPixel.Core.ImageParsers;

public class PnmImageParser : IImageParser
{
    public ImageFormat[] ImageFormats => new[] { ImageFormat.Pnm };

    public RedPixelBitmap Parse(Stream content, ColorSpaces colorSpace)
    {
        var formatHeader = new byte[2];
        content.Read(formatHeader);
        var format = new string(formatHeader.Select(x => (char) x).ToArray());

        if (format != "P5" && format != "P6")
            throw new NotSupportedException($"Unsupported image format - {format}");

        SkipSpaces(content);

        var b = content.ReadByte();
        while (b == '#')
        {
            SkipLine(content);
            b = content.ReadByte();
        }

        content.Seek(-1, SeekOrigin.Current);

        var width = ReadNumber(content);
        SkipSpaces(content);
        var height = ReadNumber(content);
        SkipSpaces(content);

        var maxColorValue = ReadNumber(content);
        _ = content.ReadByte();

        var bytesForColor = (int) Math.Log2(maxColorValue) / 8 + 1;

        var bitmap = new RedPixelBitmap(width, height, bytesForColor, colorSpace);

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                var color = ReadColor(content, format, bytesForColor);
                bitmap.SetPixel(x, y, color);
            }
        }

        return bitmap;
    }

    private void SkipSpaces(Stream content)
    {
        while (true)
        {
            var b = content.ReadByte();
            if (b == -1)
                throw new EndOfStreamException();

            if (b is ' ' or '\t' or '\r' or '\n')
                continue;

            break;
        }

        content.Seek(-1, SeekOrigin.Current);
    }

    private void SkipLine(Stream content)
    {
        while (true)
        {
            var b = content.ReadByte();
            if (b == '\n')
                break;
        }
    }

    private int ReadNumber(Stream content)
    {
        var number = new StringBuilder();

        while (true)
        {
            var b = content.ReadByte();
            if (b == -1)
                throw new EndOfStreamException();

            if (b is < '0' or > '9')
                break;

            number.Append((char) b);
        }

        content.Seek(-1, SeekOrigin.Current);
        return int.Parse(number.ToString());
    }

    private Color ReadColor(Stream content, string format, int bytesForColor)
    {
        if (format == "P5")
        {
            Span<byte> sColorBytes = stackalloc byte[bytesForColor];
            content.Read(sColorBytes);
            var color = ParseColorValue(sColorBytes);
            return new Color(color, color, color);
        }

        Span<byte> colorBytes = stackalloc byte[bytesForColor * 3];
        content.Read(colorBytes);
        var firstComponent = ParseColorValue(colorBytes.Slice(0, bytesForColor));
        var secondComponent = ParseColorValue(colorBytes.Slice(bytesForColor, bytesForColor));
        var thirdComponent = ParseColorValue(colorBytes.Slice(bytesForColor * 2));

        return new Color(firstComponent, secondComponent, thirdComponent);
    }

    private int ParseColorValue(Span<byte> colorBytes)
    {
        return colorBytes.Length switch
        {
            1 => colorBytes[0],
            2 => BitConverter.ToInt16(colorBytes),
            4 => BitConverter.ToInt32(colorBytes),
            _ => throw new NotSupportedException($"Unsupported color value length - {colorBytes.Length}")
        };
    }

    public void SerializeToStream(RedPixelBitmap image, Stream stream, ColorSpaces colorSpace, ColorComponents components)
    {
        var isGrayScale = IsGrayScale(image, components);

        var format = isGrayScale ? "P5\n" : "P6\n";
        stream.Write(Encoding.ASCII.GetBytes(format));
        stream.Write(Encoding.ASCII.GetBytes("# Created by RedPixel\n"));

        stream.Write(Encoding.ASCII.GetBytes($"{image.Width} {image.Height}\n"));

        var maxValue = 255 * image.BytesForColor;
        stream.Write(Encoding.ASCII.GetBytes($"{maxValue}\n"));

        for (int y = 0; y < image.Height; y++)
        {
            for (int x = 0; x < image.Width; x++)
            {
                var pixel = image.GetPixel(x, y);
                if (colorSpace != image.ColorSpace)
                {
                    pixel = pixel.Copy();
                    image.ColorSpace.ColorToRgb(ref pixel, components);
                    colorSpace.ColorFromRgb(ref pixel);
                }
                if (!isGrayScale)
                {
                    stream.Write((components & ColorComponents.First) != 0 ? pixel.FirstComponent.ToBytes(image.BytesForColor) : new byte[image.BytesForColor]);
                    stream.Write((components & ColorComponents.Second) != 0 ? pixel.SecondComponent.ToBytes(image.BytesForColor) : new byte[image.BytesForColor]);
                    stream.Write((components & ColorComponents.Third) != 0 ? pixel.ThirdComponent.ToBytes(image.BytesForColor) : new byte[image.BytesForColor]);
                }
                else
                {
                    byte[] value;
                    if ((components & ColorComponents.First) != 0)
                    {
                        value = pixel.FirstComponent.ToBytes(image.BytesForColor);
                    } else if ((components & ColorComponents.Second) != 0)
                    {
                        value = pixel.SecondComponent.ToBytes(image.BytesForColor);
                    }
                    else if ((components & ColorComponents.Third) != 0)
                    {
                        value = pixel.ThirdComponent.ToBytes(image.BytesForColor);
                    }
                    else
                    {
                        value = new byte[image.BytesForColor];
                    }
                    stream.Write(value);
                }
            }
        }
    }

    private bool IsGrayScale(RedPixelBitmap image, ColorComponents components)
    {
        if ((int)components == 1 || (int)components == 2 || (int)components == 4)
            return true;

        for (int y = 0; y < image.Height; y++)
        {
            for (int x = 0; x < image.Width; x++)
            {
                var color = image.GetPixel(x, y);
                if (color.FirstComponent != color.SecondComponent || color.FirstComponent != color.ThirdComponent ||
                    color.SecondComponent != color.ThirdComponent)
                    return false;
            }
        }

        return true;
    }
}