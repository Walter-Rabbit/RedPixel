using System.IO.Compression;
using System.Text;
using LibDeflate;
using RedPixel.Core.Models;

namespace RedPixel.Core.ImageParsers.Chunks;

public class PngChunk
{
    public PngChunk(Stream stream)
    {
        const int blockSize = 4;
        var block = new byte[blockSize];

        ReadBlock(stream, block, blockSize);
        Array.Reverse(block);
        Size = BitConverter.ToInt32(block);

        ReadBlock(stream, block, blockSize);
        Name = Encoding.ASCII.GetString(block);

        Content = new byte[Size];
        ReadBlock(stream, Content, Size);

        ReadBlock(stream, block, blockSize);
        Crc = BitConverter.ToInt32(block);
    }

    public int Size { get; }
    public string Name { get; }
    public byte[] Content { get; }
    public int Crc { get; }

    public bool IsCritical => char.IsUpper(Name[0]);

    public ImageInfo ParseAsIHDR()
    {
        const int blockSize = 4;
        var block = new byte[blockSize];
        
        Array.Copy(Content, block, 4);
        Array.Reverse(block);
        var width = BitConverter.ToInt32(block);
        
        Array.Copy(Content, 4, block, 0, 4);
        Array.Reverse(block);
        var height = BitConverter.ToInt32(block);
        
        Array.Clear(block);
        Array.Copy(Content, 8, block, 0, 1);
        var bytesForColor = BitConverter.ToInt32(block) / 8;

        Array.Clear(block);
        Array.Copy(Content, 9, block, 0, 1);
        var colorTypeVal = BitConverter.ToInt32(block);

        var colorType = colorTypeVal switch
        {
            0 => ColorTypes.Halftone,
            2 => ColorTypes.Rgb,
            3 => ColorTypes.Palette,
            4 => ColorTypes.HalftoneAlpha,
            6 => ColorTypes.RgbAlpha,
            _ => throw new Exception("TODO")
        };
        
        Array.Clear(block);
        Array.Copy(Content, 10, block, 0, 3);
        var value = BitConverter.ToInt32(block);

        if (value != 0)
        {
            throw new Exception("TODO");
        }

        //TODO: implement parsing

        return new ImageInfo(width, height, bytesForColor, colorType, 0, 0, 0);
    }
    
    private static void ReadBlock(Stream stream, byte[] block, int blockSize)
    {
        var totalRead = 0;
        while (totalRead < blockSize)
        {
            var read = stream.Read(block, totalRead, blockSize - totalRead);
            totalRead += read;
            if (read == 0) break;
        }

        // TODO: implements new exception
        if (totalRead < blockSize) throw new Exception("");
    }
}