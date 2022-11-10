using System.Text;
using RedPixel.Core.Colors;
using RedPixel.Core.Colors.ValueObjects;
using RedPixel.Core.Tools;
using RedPixelBitmap = RedPixel.Core.Bitmap.Bitmap;

namespace RedPixel.Core.ImageParsers;

public class PnmImageParser : IImageParser
{
    public ImageFormat[] ImageFormats => new[] { ImageFormat.Pnm };

    public Bitmap.Bitmap Parse(Stream content, ColorSpace space)
    {
        var formatHeader = new byte[2];
        content.Read(formatHeader);
        var format = new string(formatHeader.Select(x => (char)x).ToArray());

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

        var bytesForColor = (int)Math.Log2(maxColorValue) / 8 + 1;

        var bitmap = new RedPixelBitmap(width, height);

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                var color = ReadColor(content, format, bytesForColor, space);
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

            number.Append((char)b);
        }

        content.Seek(-1, SeekOrigin.Current);
        return int.Parse(number.ToString());
    }

    private IColor ReadColor(Stream content, string format, int bytesForColor, ColorSpace colorSpace)
    {
        if (format == "P5")
        {
            Span<byte> sColorBytes = stackalloc byte[bytesForColor];
            content.Read(sColorBytes);
            var color = ParseColorValue(sColorBytes);
            return colorSpace.Creator.Invoke(color, color, color, bytesForColor);
        }

        Span<byte> colorBytes = stackalloc byte[bytesForColor * 3];
        content.Read(colorBytes);
        var firstComponent = ParseColorValue(colorBytes.Slice(0, bytesForColor));
        var secondComponent = ParseColorValue(colorBytes.Slice(bytesForColor, bytesForColor));
        var thirdComponent = ParseColorValue(colorBytes.Slice(bytesForColor * 2));

        return colorSpace.Creator.Invoke(firstComponent, secondComponent, thirdComponent, bytesForColor);
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

    public void SerializeToStream(
        RedPixelBitmap image, 
        Stream stream, 
        ColorSpace colorSpace,
        ColorComponents components)
    {
        var bitmap = new RedPixelBitmap(image);

        var isGrayScale = IsGrayScale(bitmap, components);

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
                var color = colorSpace.Converter.Invoke(bitmap.GetPixel(x, y));
                if (!isGrayScale)
                {
                    stream.Write((components & ColorComponents.First) != 0
                        ? color.FirstComponent.ToBytes(color.BytesForColor)
                        : new byte[color.BytesForColor]);
                    stream.Write((components & ColorComponents.Second) != 0
                        ? color.SecondComponent.ToBytes(color.BytesForColor)
                        : new byte[color.BytesForColor]);
                    stream.Write((components & ColorComponents.Third) != 0
                        ? color.ThirdComponent.ToBytes(color.BytesForColor)
                        : new byte[color.BytesForColor]);
                }
                else
                {
                    byte[] value;
                    if ((components & ColorComponents.First) != 0)
                    {
                        value = color.FirstComponent.ToBytes(color.BytesForColor);
                    }
                    else if ((components & ColorComponents.Second) != 0)
                    {
                        value = color.SecondComponent.ToBytes(color.BytesForColor);
                    }
                    else if ((components & ColorComponents.Third) != 0)
                    {
                        value = color.ThirdComponent.ToBytes(color.BytesForColor);
                    }
                    else
                    {
                        value = new byte[color.BytesForColor];
                    }

                    stream.Write(value);
                }
            }
        }
    }

    private bool IsGrayScale(Bitmap.Bitmap image, ColorComponents components)
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