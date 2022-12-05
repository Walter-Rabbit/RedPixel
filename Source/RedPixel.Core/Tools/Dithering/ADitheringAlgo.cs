using RedPixel.Core.Colors.ValueObjects;

namespace RedPixel.Core.Tools.Dithering;

public abstract class ADitheringAlgo
{
    protected static Color FindClosestPaletteColor(Color clr, ColorDepth depth)
    {
        var firstDelta = (int)Math.Pow(2, 8 - depth.FirstComponent);
        var secondDelta = (int)Math.Pow(2, 8 - depth.SecondComponent);
        var thirdDelta = (int)Math.Pow(2, 8 - depth.ThirdComponent);

        var firstColor = (int)clr.FirstComponent / firstDelta * firstDelta * 2;
        var secondColor = (int)clr.SecondComponent / secondDelta * secondDelta * 2;
        var thirdColor = (int)clr.ThirdComponent / thirdDelta * thirdDelta * 2;

        return Normalize(new Color(firstColor, secondColor, thirdColor));
    }

    protected static Color GetError(Color lhs, Color rhs)
    {
        return new Color(
            lhs.FirstComponent - rhs.FirstComponent,
            lhs.SecondComponent - rhs.SecondComponent,
            lhs.ThirdComponent - rhs.ThirdComponent
        );
    }

    protected static Color GetPixelWithError(Color clr, Color error, float weight)
    {
        var first = clr.FirstComponent + error.FirstComponent * weight;
        var second = clr.SecondComponent + error.SecondComponent * weight;
        var third = clr.ThirdComponent + error.ThirdComponent * weight;

        return Normalize(new Color(first, second, third));
    }

    protected static Color Normalize(Color clr)
    {
        var firstColor = Math.Min(Math.Max(clr.FirstComponent, 0), 255);
        var secondColor = Math.Min(Math.Max(clr.SecondComponent, 0), 255);
        var thirdColor = Math.Min(Math.Max(clr.ThirdComponent, 0), 255);

        return new Color(firstColor, secondColor, thirdColor);
    }
}