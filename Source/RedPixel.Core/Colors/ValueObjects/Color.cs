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

    public static Color operator *(Color color, float multiplier)
    {
        return new Color(color.FirstComponent * multiplier, color.SecondComponent * multiplier, color.ThirdComponent * multiplier);
    }

    public static Color operator +(Color color1, Color color2)
    {
        return new Color(color1.FirstComponent + color2.FirstComponent, color1.SecondComponent + color2.SecondComponent, color1.ThirdComponent + color2.ThirdComponent);
    }

    public Color Normalize()
    {
        var firstComponent = Math.Min(Math.Max(FirstComponent, 0), 255);
        var secondComponent = Math.Min(Math.Max(SecondComponent, 0), 255);
        var thirdComponent = Math.Min(Math.Max(ThirdComponent, 0), 255);

        return new Color(firstComponent, secondComponent, thirdComponent);
    }
}