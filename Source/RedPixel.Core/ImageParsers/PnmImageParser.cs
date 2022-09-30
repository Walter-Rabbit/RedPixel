using System.Drawing;
using System.Text;

namespace RedPixel.Core.ImageParsers;

public class PnmImageParser : IImageParser
{
    public ImageFormat[] ImageFormats => new[] { ImageFormat.Pnm };

    public Image Parse(Stream content)
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

        //TODO: hack?
        var bytesForColor = maxColorValue > 255 ? 2 : 1;
        var bitmap = new Bitmap(width, height);
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

            number.Append((char)b);
        }

        content.Seek(-1, SeekOrigin.Current);
        return int.Parse(number.ToString());
    }

    private Color ReadColor(Stream content, string format, int bytesForColor)
    {
        var colorBytes = new byte[bytesForColor];
        if (format == "P5")
        {
            content.Read(colorBytes);
            var color = ParseColorValue(colorBytes);
            return Color.FromArgb(color, color, color);
        }

        content.Read(colorBytes);
        var red = ParseColorValue(colorBytes);
        content.Read(colorBytes);
        var green = ParseColorValue(colorBytes);
        content.Read(colorBytes);
        var blue = ParseColorValue(colorBytes);

        return Color.FromArgb(red, green, blue);
    }

    public int ParseColorValue(byte[] colorBytes)
    {
        return colorBytes.Length switch
        {
            1 => colorBytes[0],
            2 => BitConverter.ToInt16(colorBytes),
            4 => BitConverter.ToInt32(colorBytes),
            _ => throw new NotSupportedException($"Unsupported color value length - {colorBytes.Length}")
        };
    }

    public void SerializeToStream(Image image, Stream stream)
    {
        var isColored = image.Palette.Entries.Any(x => x.R != x.G || x.G != x.B || x.B != x.R);

        var format = isColored ? "P6" : "P5";
        using var writer = new StreamWriter(stream);
        writer.WriteLine(format);
        writer.WriteLine("# Created by RedPixel");
        writer.WriteLine($"{image.Width} {image.Height}");

        //TODO : hack
        writer.WriteLine("255");
        writer.Flush();

        var bitmap = new Bitmap(image);
        for (int y = 0; y < image.Height; y++)
        {
            for (int x = 0; x < image.Width; x++)
            {
                var color = bitmap.GetPixel(x, y);
                if (isColored)
                {
                    stream.WriteByte(color.R);
                    stream.WriteByte(color.G);
                    stream.WriteByte(color.B);
                }
                else
                {
                    stream.WriteByte(color.R);
                }
            }
        }
    }
}