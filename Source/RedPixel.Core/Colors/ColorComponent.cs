namespace RedPixel.Core.Colors;

public class ColorComponent
{
    public ColorComponent(float value)
    {
        Value = value;
    }

    public float Value { get; }
    public bool Visible { get; set; } = true;

    public byte ByteValue => Visible ? (byte)Value : (byte)0;
}