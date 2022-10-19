using System.Text;

namespace RedPixel.Core.Colors;

public abstract class Color
{
    private bool _firstComponentShown = true;
    private bool _secondComponentShown = true;
    private bool _thirdComponentShown = true;

    protected abstract byte FirstComponentValue { get; }
    protected abstract byte SecondComponentValue { get; }
    protected abstract byte ThirdComponentValue { get; }

    public byte FirstComponent => _firstComponentShown ? FirstComponentValue : (byte)0;
    public byte SecondComponent => _secondComponentShown ? SecondComponentValue : (byte)0;
    public byte ThirdComponent => _thirdComponentShown ? ThirdComponentValue : (byte)0;

    public abstract Color Create(float firstComponent, float secondComponent, float thirdComponent);

    public void SelectComponents(ColorComponents components)
    {
        _firstComponentShown = (components & ColorComponents.First) != 0;
        _secondComponentShown = (components & ColorComponents.Second) != 0;
        _thirdComponentShown = (components & ColorComponents.Third) != 0;
    }

    public abstract RgbColor ToRgb();
    public virtual System.Drawing.Color ToSystemColor() => ToRgb().ToSystemColor();
    public virtual HSVColor ToHsv() => ToRgb().ToHsv();
}