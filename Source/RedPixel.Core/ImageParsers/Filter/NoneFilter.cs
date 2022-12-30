using RedPixel.Core.Colors.ValueObjects;

namespace RedPixel.Core.ImageParsers.Filter;

public class NoneFilter : IFilter
{
    public void DoFiltration(Color[] line, Color[] prevLine)
    { }
}