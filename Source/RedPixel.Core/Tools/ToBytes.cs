namespace RedPixel.Core.Tools;

public static class FloatExtensions
{
    public static byte[] ToBytes(this float val, int byteSize)  {
        return byteSize switch
            {
                1 => new byte[] { (byte)val },
                2 => BitConverter.GetBytes((short)val),
                4 => BitConverter.GetBytes((int)val),
                _ => throw new ArgumentOutOfRangeException(nameof(byteSize), byteSize, null)
            };
    }
}