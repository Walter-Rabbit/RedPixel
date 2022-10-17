namespace RedPixel.Core.Colors;

using SystemColor  = System.Drawing.Color;

public class RgbColor : Color
{
    private readonly float _r;
    private readonly float _g;
    private readonly float _b;

    public override byte FirstComponent => (byte)_r;
    // G
    public override byte SecondComponent => (byte)_g;
    // B
    public override byte ThirdComponent => (byte)_b;

    public RgbColor(float r, float g, float b)
    {
        _r = r;
        _g = g;
        _b = b;
    }

    public override Color Create(float firstComponent, float secondComponent, float thirdComponent)
    {
        return new RgbColor(firstComponent, secondComponent, thirdComponent);
    }

    public override SystemColor ToSystemColor()
    {
        return SystemColor.FromArgb(FirstComponent, SecondComponent, ThirdComponent);
    }

    public override RgbColor ToRgb()
    {
        return this;
    }
}