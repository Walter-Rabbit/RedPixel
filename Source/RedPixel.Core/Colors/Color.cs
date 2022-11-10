using RedPixel.Core.Colors.ValueObjects;

namespace RedPixel.Core.Colors;
public struct Color
{
    public float FirstComponent { get; set; }
    public float SecondComponent { get; set; }
    public float ThirdComponent { get; set; }

    public Color(float firstComponent, float secondComponent, float thirdComponent)
    {
        FirstComponent = firstComponent;
        SecondComponent = secondComponent;
        ThirdComponent = thirdComponent;
    }

    public Color Copy()
    {
        return new Color(FirstComponent, SecondComponent, ThirdComponent);
    }
}