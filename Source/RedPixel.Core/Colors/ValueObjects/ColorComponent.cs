namespace RedPixel.Core.Colors.ValueObjects;

public record ColorComponent(float Value, int ByteSize)
{
    public byte[] BytesValue  {
        get
        {
            return ByteSize switch
            {
                1 => new byte[] { (byte)Value },
                2 => BitConverter.GetBytes((short)Value),
                4 => BitConverter.GetBytes((int)Value),
                _ => throw new ArgumentOutOfRangeException(nameof(ByteSize), ByteSize, null)
            };
        }
    }
}