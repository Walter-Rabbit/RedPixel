namespace RedPixel.Core.Colors;

public abstract class Color
{
    public abstract byte FirstComponent { get; }
    public abstract byte SecondComponent { get; }
    public abstract byte ThirdComponent { get; }

    public abstract Color Create(float firstComponent, float secondComponent, float thirdComponent);

    public Color SelectComponents(ColorComponents components)
    {
        var firstComponent = components.HasFlag(ColorComponents.First) ? FirstComponent : (byte)0;
        var secondComponent = components.HasFlag(ColorComponents.Second) ? SecondComponent : (byte)0;
        var thirdComponent = components.HasFlag(ColorComponents.Third) ? ThirdComponent : (byte)0;

        if ((components & (components -1)) != 0) return Create(firstComponent, secondComponent, thirdComponent);

        var component = firstComponent + secondComponent + thirdComponent;
        return Create(component, component, component);

    }

    public abstract System.Drawing.Color ToSystemColor();
    public abstract RgbColor ToRgb();
}