namespace RedPixel.Core.Colors.ValueObjects;

public class ColorDepth
{
    public ColorDepth(int firstComponent, int secondComponent, int thirdComponent)
    {
        FirstComponent = firstComponent;
        SecondComponent = secondComponent;
        ThirdComponent = thirdComponent;
    }

    public int FirstComponent { get; set; }
    public int SecondComponent { get; set; }
    public int ThirdComponent { get; set; }
}