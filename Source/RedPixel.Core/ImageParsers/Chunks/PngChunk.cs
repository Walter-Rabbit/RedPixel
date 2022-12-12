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
        Name = System.Text.Encoding.ASCII.GetString(block);

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

    private static void ReadBlock(Stream stream, byte[] block, int blockSize)
    {
        var totalRead = 0;
        while (totalRead < blockSize)
        {
            var read = stream.Read(block, totalRead, blockSize - totalRead);
            totalRead += read;
            if (read == 0)
            {
                break;
            }
        }

        // TODO: implements new exception
        if (totalRead < blockSize)
        {
            throw new Exception("");
        }
    }
}