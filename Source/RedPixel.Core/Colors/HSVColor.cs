namespace RedPixel.Core.Colors;

public class HSVColor : Color
{
    private readonly float _hue;
    private readonly float _saturation;
    private readonly float _value;

    public HSVColor(float hue, float saturation, float value)
    {
        _hue = hue;
        _saturation = saturation;
        _value = value;
    }

    protected override byte FirstComponentValue => (byte)(_hue * 2.55);
    protected override byte SecondComponentValue => (byte)(_saturation * 2.55);
    protected override byte ThirdComponentValue => (byte)(_value * 2.55);
    public override Color Create(float firstComponent, float secondComponent, float thirdComponent)
    {
        return new HSVColor(firstComponent, secondComponent, thirdComponent);
    }

    public override RgbColor ToRgb()
    {
        var hi = (int)(_hue / 60) % 6;
        var vmin = (1 - _saturation) * _value / 100;
        var a = (_value - vmin) * (_hue % 60) / 60;
        var vinc = vmin + a;
        var vdec = _value - a;

        return hi switch {
            0 => new RgbColor(_value, vinc, vmin),
            1 => new RgbColor(vdec, _value, vmin),
            2 => new RgbColor(vmin, _value, vinc),
            3 => new RgbColor(vmin, vdec, _value),
            4 => new RgbColor(vinc, vmin, _value),
            5 => new RgbColor(_value, vmin, vdec),
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    public override HSVColor ToHsv() => this;
}