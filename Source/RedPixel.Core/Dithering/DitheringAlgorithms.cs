using System.Reflection;
using RedPixel.Core.Models;

namespace RedPixel.Core.Dithering;

public class DitheringAlgorithms
{
    public delegate void InFunc<T>(T arg);
    
    public string Name { get; }
    
    public InFunc<Bitmap> ApplyDithering { get; }

    public DitheringAlgorithms(
        string name,
        InFunc<Bitmap> applyDithering)
    {
        Name = name;
        ApplyDithering = applyDithering;
    }

    public static DitheringAlgorithms RawConversion = new DitheringAlgorithms(
        "RawConversion",
        RawConversionDithering.ApplyDithering);
    
    public static DitheringAlgorithms RandomConversion = new DitheringAlgorithms(
        "Random",
        RandomDithering.ApplyDithering);
    
    public static DitheringAlgorithms FloydSteinbergConversion = new DitheringAlgorithms(
        "Floyd Steinberg",
        FloydSteinbergDithering.ApplyDithering);
    
    public static DitheringAlgorithms AtkinsonConversion = new DitheringAlgorithms(
        "Atkinson",
        AtkinsonDithering.ApplyDithering);
    
    public static Lazy<IEnumerable<DitheringAlgorithms>> AllAlgorithms => new(
        () => typeof(DitheringAlgorithms)
            .GetFields(BindingFlags.Public | BindingFlags.Static)
            .Where(f => f.FieldType == typeof(DitheringAlgorithms))
            .Select(f => (DitheringAlgorithms)f.GetValue(null))
    );
}