namespace RedPixel.Core.Colors;

using SystemColor  = System.Drawing.Color;

public class RgbColor : Color
{
    private readonly float _r;
    private readonly float _g;
    private readonly float _b;

    protected override byte FirstComponentValue => (byte)_r;
    protected override byte SecondComponentValue => (byte)_g;
    protected override byte ThirdComponentValue => (byte)_b;

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

    public override RgbColor ToRgb() => this;

    public override HSVColor ToHsv()
    {
        var max = Math.Max(_r, Math.Max(_g, _b));
        var min = Math.Min(_r, Math.Min(_g, _b));

        var h = 0f;
        var s = 0f;
        var v = max;

        var d = max - min;
        if (max != 0)
            s = d / max;

        if (s != 0)
        {
            if (_r == max)
                h = (_g - _b) / d;
            else if (_g == max)
                h = 2 + (_b - _r) / d;
            else if (_b == max)
                h = 4 + (_r - _g) / d;

            h *= 60;
            if (h < 0)
                h += 360;
        }

        return new HSVColor(h, s, v);
    }
}