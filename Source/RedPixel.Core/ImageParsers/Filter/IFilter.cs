namespace RedPixel.Core.ImageParsers.Filter;
using RedPixel.Core.Colors;
using RedPixel.Core.Colors.ValueObjects;

public interface IFilter
{
    void DoFiltration(Color[] line, Color[] prevLine);
}