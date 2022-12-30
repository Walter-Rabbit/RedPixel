using RedPixel.Core.Colors.ValueObjects;
using RedPixel.Core.Tools.Utilities;

namespace RedPixel.Core.ImageParsers.Filter;

public class PaethFilter : IFilter
{
    public void DoFiltration(Color[] line, Color[] prevLine)
    {
        for (var i = 0; i < line.Length; ++i)
        {
            var leftClr = i != 0 ? line[i - 1] : new Color(0, 0, 0);
            var prevLeftClr = i != 0 ? prevLine[i - 1] : new Color(0, 0, 0);
            line[i] = Normalizer.Normalize(new Color(
                line[i].FirstComponent + 
                CalculateDelta(leftClr.FirstComponent, prevLine[i].FirstComponent, prevLeftClr.FirstComponent),
                line[i].SecondComponent +
                CalculateDelta(leftClr.SecondComponent, prevLine[i].SecondComponent, prevLeftClr.SecondComponent),
                line[i].ThirdComponent +
                CalculateDelta(leftClr.ThirdComponent, prevLine[i].ThirdComponent, prevLeftClr.ThirdComponent)
            ));
        }
    }

    private static float CalculateDelta(float a, float b, float c)
    {
        
        var p = (int)(a + b - c);

        var pa = Math.Abs(p - (int)a);
        var pb = Math.Abs(p - (int)b);
        var pc = Math.Abs(p - (int)c);

        if (pa <= pb && pa <= pc)
        {
            return a;
        } 
        return pb <= pc ? b : c;
    }

}