using System.Buffers.Binary;
using System.Diagnostics;
using System.IO.Compression;
using RedPixel.Core.Colors;
using RedPixel.Core.Colors.ValueObjects;
using RedPixel.Core.ImageParsers.Chunks;
using RedPixel.Core.ImageParsers.Filter;
using RedPixel.Core.Models;
using RedPixelBitmap = RedPixel.Core.Models.Bitmap;

namespace RedPixel.Core.ImageParsers;

public class PngImageParser : IImageParser
{
    private List<PngChunk> _chunks;
    private readonly string[] _criticalChunks = { "IHDR", "PLTE", "IDAT", "IEND" };
    private bool _palette;
    private List<Color> _colors;

    public PngImageParser()
    {
        _palette = false;
        _colors = new List<Color>();
    }
    
    public ImageFormat[] ImageFormats => new[] { ImageFormat.Png };

    public RedPixelBitmap Parse(Stream content, ColorSpaces colorSpaces)
    {
        _chunks = new List<PngChunk>();
        if (!ImageFormat.Png.IsMatch(content)) throw new NotSupportedException($"Unsupported image format - {content}");
        content.Seek(8, SeekOrigin.Current);
        
        ReadAllChunks(content);

        var (width, height, bytesForColor, colorType, _, _, _) = _chunks[0].ParseAsIHDR();
        var bitmap = new RedPixelBitmap(width, height, bytesForColor, ColorSpaces.Rgb);

        var plte = _chunks.FirstOrDefault(item => item.Name == "PLTE");
        if (plte is not null)
        {
            for (var i = 0; i < plte.Content.Length; i += 3)
            {
                _colors.Add(new Color(
                    plte.Content[i], 
                    plte.Content[i + 1],
                    plte.Content[i + 2]));
            }
        }

        using var idatContent = new MemoryStream();
        foreach (var chunk in _chunks.Where(chunk => chunk.Name == "IDAT"))
        {
            idatContent.Write(chunk.Content);
        }

        idatContent.Position = 0;

        using var output = new MemoryStream();
        using var ds = new ZLibStream(idatContent, CompressionMode.Decompress);
        ds.CopyTo(output);
        output.Position = 0;

        var prevLine = new Color[width];
        var line = new Color[width];
        
        
        for (var y = 0; y < height; y++)
        {
            var firstByte = output.ReadByte();
            

            IFilter filter = firstByte switch
            {
                0x00 => new NoneFilter(),
                0x01 => new SubFilter(),
                0x02 => new UpFilter(),
                0x03 => new AverageFilter(),
                0x04 => new PaethFilter(),
                _ => throw new ArgumentOutOfRangeException()
            };
            
            Debug.WriteLine($"{y}: {firstByte}");
            
            for (var x = 0; x < width; x++)
            { 
                Color color;
                if (colorType == ColorTypes.Palette)
                {
                    var index = output.ReadByte();
                    color = _colors[index];
                }
                else
                {
                    color = ReadColor(output, bytesForColor, colorType);
                }
                line[x] = color;
            }
            
            filter.DoFiltration(line, prevLine);
            for (var x = 0; x < width; x++)
            {
                bitmap.SetPixel(x, y, line[x]);
            }
            
            Array.Copy(line, prevLine, line.Length);
        }
        
        return bitmap;
    }

    public void SerializeToStream(RedPixelBitmap image, Stream stream, ColorSpaces colorSpaces,
        ColorComponents components)
    {
        stream.Write(new byte[] { 137, 80, 78, 71, 13, 10, 26, 10 });
        SerializeIhdr(stream, image.Width, image.Height, ColorTypes.Rgb);
        SerializeGama(stream, 0f);
        
        SerializeIdat(stream, image, ColorTypes.Rgb);
        
        SerializeIend(stream);
        stream.Position = 0;
    }

    private void ReadAllChunks(Stream content)
    {
        _chunks.Add(new PngChunk(content));
        while (_chunks.Last().Name != "IEND") _chunks.Add(new PngChunk(content));
        
        ValidateChunks(content);
    }

    private void ValidateChunks(Stream content)
    {
        if (_chunks.Count < 1 && _chunks[0].Name != "IHDR")
        {
            throw new Exception("first chunk must be IHDR");
        }
        
    }

    private Color ReadColor(Stream content, int bytesForColor, ColorTypes colorType)
    {
        Span<byte> colorBytes = stackalloc byte[bytesForColor * 3];
        content.Read(colorBytes);
        var firstComponent = ParseColorValue(colorBytes.Slice(0, bytesForColor));
        var secondComponent = ParseColorValue(colorBytes.Slice(bytesForColor, bytesForColor));
        var thirdComponent = ParseColorValue(colorBytes.Slice(bytesForColor * 2));
        
        switch (colorType)
        {
            case ColorTypes.Rgb:
                return new Color(firstComponent, secondComponent, thirdComponent);
                break;
            case ColorTypes.RgbAlpha:
                content.Seek(1, SeekOrigin.Current);
                return new Color(firstComponent, secondComponent, thirdComponent);
                break;
        }

        throw new NotImplementedException("this type not supported yet");
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
    private void SerializeIhdr(
        Stream stream, 
        int Width, 
        int Height,
        ColorTypes ColorType)
    {
        stream.Write(BitConverter.GetBytes(BinaryPrimitives.ReverseEndianness((uint)13)), 0, 4);
        stream.Write("IHDR"u8);

        var buffer = new byte[13];
        buffer[8] = 8;
        buffer[9] = (byte)ColorType;
        buffer[10] = 0;
        buffer[11] = 0;
        buffer[12] = 0;

        BitConverter.GetBytes(BinaryPrimitives.ReverseEndianness(Width)).CopyTo(buffer, 0);
        BitConverter.GetBytes(BinaryPrimitives.ReverseEndianness(Height)).CopyTo(buffer, 4);
        stream.Write(buffer);
        stream.Write(BitConverter.GetBytes(BinaryPrimitives.ReverseEndianness(CalculateCrc(
            "IHDR"u8.ToArray(), buffer
            ))));
    }
    
    private void SerializeIdat(Stream stream, RedPixelBitmap image, ColorTypes ColorType)
    {
        var height = image.Height;
        var width = image.Width;

        using var ms = new MemoryStream();
        for (var y = 0; y < height; y++)
        {
            ms.WriteByte(0);
            for (var x = 0; x < width; x++)
            {
                var color = image.GetPixel(x, y);
                ms.WriteByte((byte)color.FirstComponent);

                if (ColorType != ColorTypes.Rgb) continue;
                
                ms.WriteByte((byte)color.SecondComponent);
                ms.WriteByte((byte)color.ThirdComponent);
            }
        }
        
        using var cms = new MemoryStream();
        ms.Seek(0, SeekOrigin.Begin);
        
        using (var deflateStream = new ZLibStream(cms, CompressionLevel.Optimal, true))
        {
            ms.CopyTo(deflateStream);
        }
        
        var idatData = cms.ToArray();
        stream.Write(BitConverter.GetBytes(BinaryPrimitives.ReverseEndianness(
            idatData.Length
        )));
        stream.Write("IDAT"u8);
        stream.Write(idatData);
        stream.Write(
            BitConverter.GetBytes(BinaryPrimitives.ReverseEndianness(CalculateCrc(
                "IDAT"u8.ToArray(), idatData))));
    }

    private void SerializeGama(Stream stream, float gamma)
    {
        stream.Write(BitConverter.GetBytes(BinaryPrimitives.ReverseEndianness(4)));
        stream.Write("gAMA"u8);

        if (Math.Abs(gamma) >= 2 * float.Epsilon)
        {
            var exactGamma = (uint)(float.Abs(gamma + 1) < float.Epsilon ? 45455 : gamma * 100000);
            var parsedGamma = BitConverter.GetBytes(BinaryPrimitives.ReverseEndianness(exactGamma));

            stream.Write(parsedGamma, 0, parsedGamma.Length);
            stream.Write(BitConverter.GetBytes(BinaryPrimitives.ReverseEndianness(CalculateCrc("gAMA"u8.ToArray(), parsedGamma))));
        }
        else
        {
            stream.Write(new byte[] {0x0, 0x0, 0xB1, 0x8F});
            stream.Write(BitConverter.GetBytes(BinaryPrimitives.ReverseEndianness(CalculateCrc("gAMA"u8.ToArray(), new byte[] {0x0, 0x0, 0xB1, 0x8F}))));
        }
    }
    private void SerializeIend(Stream stream)
    {
        stream.Write(BitConverter.GetBytes(BinaryPrimitives.ReverseEndianness((uint)0)), 0, 4);
        stream.Write("IEND"u8);
        stream.Write(
            BitConverter.GetBytes(BinaryPrimitives.ReverseEndianness(CalculateCrc(
                "IEND"u8.ToArray(), Array.Empty<byte>()))));
    }

    private static uint CalculateCrc(IEnumerable<byte> chunkType, byte[] chunkData)
    {
        var crc = chunkType.Aggregate(0xffffffff, UpdateCrc);
        crc = chunkData.Aggregate(crc, UpdateCrc);

        return crc ^ 0xffffffff;
    }
    
    private static uint UpdateCrc(uint crc, byte b)
    {
        crc ^= b;
        for (var i = 0; i < 8; i++)
        {
            if ((crc & 1) != 0)
            {
                crc = (crc >> 1) ^ 0xedb88320;
            }
            else
            {
                crc >>= 1;
            }
        }

        return crc;
    }
}