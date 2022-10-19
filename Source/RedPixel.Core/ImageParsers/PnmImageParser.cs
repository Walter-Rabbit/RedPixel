using System.Diagnostics;
using System.Text;
using RedPixel.Core.Colors;
using RedPixelBitmap = RedPixel.Core.Bitmap.Bitmap;

namespace RedPixel.Core.ImageParsers;

public class PnmImageParser : IImageParser
{
    public ImageFormat[] ImageFormats => new[] { ImageFormat.Pnm };

    public Bitmap.Bitmap Parse(Stream content)
    {
        var sw = new Stopwatch();
        sw.Start();
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

        var bitmap = new RedPixelBitmap(width, height);
        File.AppendAllText("time-log.txt", $"Headers read {sw.ElapsedMilliseconds} ms{Environment.NewLine}");
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                var color = ReadColor(content, format, bytesForColor);
                bitmap.SetPixel(x, y, color);
            }
        }

        sw.Stop();
        File.AppendAllText("time-log.txt", $"Image reading took {sw.ElapsedMilliseconds} ms{Environment.NewLine}");

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

    private RgbColor ReadColor(Stream content, string format, int bytesForColor)
    {
        if (format == "P5")
        {
            Span<byte> sColorBytes = stackalloc byte[bytesForColor];
            content.Read(sColorBytes);
            var color = ParseColorValue(sColorBytes);
            return new RgbColor(color, color, color);
        }

        Span<byte> colorBytes = stackalloc byte[bytesForColor*3];
        content.Read(colorBytes);
        var red = ParseColorValue(colorBytes.Slice(0, bytesForColor));
        var green = ParseColorValue(colorBytes.Slice(bytesForColor, bytesForColor*2));
        var blue = ParseColorValue(colorBytes.Slice(bytesForColor*2));

        // TODO: Fix
        return new RgbColor(red, green, blue);
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

    public void SerializeToStream(RedPixelBitmap image, Stream stream)
    {
        var bitmap = new RedPixelBitmap(image);

        var isGrayScale = IsGrayScale(bitmap);

        var format = isGrayScale ? "P5\n" : "P6\n";
        stream.Write(Encoding.ASCII.GetBytes(format));
        stream.Write(Encoding.ASCII.GetBytes("# Created by RedPixel\n"));

        stream.Write(Encoding.ASCII.GetBytes($"{image.Width} {image.Height}\n"));

        stream.Write(Encoding.ASCII.GetBytes("255\n"));

        for (int y = 0; y < image.Height; y++)
        {
            for (int x = 0; x < image.Width; x++)
            {
                var color = bitmap.GetPixel(x, y);
                if (!isGrayScale)
                {
                    stream.WriteByte(color.FirstComponent);
                    stream.WriteByte(color.SecondComponent);
                    stream.WriteByte(color.ThirdComponent);
                }
                else
                {
                    stream.WriteByte(color.FirstComponent);
                }
            }
        }
    }

    private bool IsGrayScale(Bitmap.Bitmap image)
    {
        for (int y = 0; y < image.Height; y++)
        {
            for (int x = 0; x < image.Width; x++)
            {
                var color = image.GetPixel(x, y);
                if (color.FirstComponent != color.SecondComponent || color.FirstComponent != color.ThirdComponent || color.SecondComponent != color.ThirdComponent)
                    return false;
            }
        }

        return true;
    }
}