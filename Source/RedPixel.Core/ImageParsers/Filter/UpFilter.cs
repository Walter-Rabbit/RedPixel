using RedPixel.Core.Colors.ValueObjects;
using RedPixel.Core.Tools.Utilities;

namespace RedPixel.Core.ImageParsers.Filter;

public class UpFilter : IFilter
{
    public void DoFiltration(Color[] line, Color[] prevLine)
    {
        for (var i = 0; i < line.Length; ++i)
        {
            line[i] = Normalizer.Normalize(new Color(
                line[i].FirstComponent + prevLine[i].FirstComponent,
                line[i].SecondComponent + prevLine[i].SecondComponent,
                line[i].ThirdComponent + prevLine[i].ThirdComponent
            ));
        }
    }
}