namespace RedPixel.Core.Colors.ValueObjects;

public class ColorComponent
{
    public ColorComponent(float value)
    {
        Value = value;
    }

    public float Value { get; }
    public bool Visible { get; set; } = true;

    public byte[] BytesValue => Visible ? BitConverter.GetBytes((int) Value) : BitConverter.GetBytes(0);
}