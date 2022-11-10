using RedPixel.Core.Colors.ValueObjects;

namespace RedPixel.Core.Colors;
public readonly struct Color
{
    public float FirstComponent { get; }
    public float SecondComponent { get; }
    public float ThirdComponent { get; }

    public Color(float firstComponent, float secondComponent, float thirdComponent)
    {
        FirstComponent = firstComponent;
        SecondComponent = secondComponent;
        ThirdComponent = thirdComponent;
    }
}