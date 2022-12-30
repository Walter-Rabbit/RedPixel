using RedPixel.Core.Colors.ValueObjects;
using RedPixel.Core.Tools.Utilities;

namespace RedPixel.Core.ImageParsers.Filter;

public class SubFilter : IFilter
{
    public void DoFiltration(Color[] line, Color[] prevLine)
    {
        for (var i = 1; i < line.Length; ++i)
        {
            line[i] = Normalizer.Normalize(new Color(
                line[i].FirstComponent + line[i - 1].FirstComponent,
                line[i].SecondComponent + line[i - 1].SecondComponent,
                line[i].ThirdComponent + line[i - 1].ThirdComponent
            ));
        }
    }
}