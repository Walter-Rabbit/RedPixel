namespace RedPixel.Core.Colors.ValueObjects;

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