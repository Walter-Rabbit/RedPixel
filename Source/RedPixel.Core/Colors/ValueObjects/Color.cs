namespace RedPixel.Core.Colors.ValueObjects;

public readonly struct Color
{
    public float FirstComponent { get; }
    public float SecondComponent { get; }
    public float ThirdComponent { get; }

    public float this[int component]
    {
        get
        {
            return component switch
            {
                0 => FirstComponent,
                1 => SecondComponent,
                2 => ThirdComponent,
                _ => throw new ArgumentException("There is only 3 color components")
            };
        }
    }

    public Color(float firstComponent, float secondComponent, float thirdComponent)
    {
        FirstComponent = firstComponent;
        SecondComponent = secondComponent;
        ThirdComponent = thirdComponent;
    }
}