using RedPixel.Core.Colors.ValueObjects;
using RedPixel.Core.Tools.Utilities;

namespace RedPixel.Core.ImageParsers.Filter;

public class AverageFilter : IFilter
{
    public void DoFiltration(Color[] line, Color[] prevLine)
    {
        for (var i = 0; i < line.Length; ++i)
        {
            var leftClr = i != 0 ? line[i - 1] : new Color(0, 0, 0);
            line[i] = Normalizer.Normalize(new Color(
                line[i].FirstComponent + CalculateDelta(leftClr.FirstComponent, prevLine[i].FirstComponent),
                line[i].SecondComponent + CalculateDelta(leftClr.SecondComponent, prevLine[i].SecondComponent),
                line[i].ThirdComponent + CalculateDelta(leftClr.ThirdComponent, prevLine[i].ThirdComponent)
            ));
        }
    }
    
    private static float CalculateDelta(float a, float b)
    {
        return float.Floor((a + b) / 2);
    }
}