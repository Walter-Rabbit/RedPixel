using System.Reflection;
using RedPixel.Core.Colors.ValueObjects;
using RedPixel.Core.Models;

namespace RedPixel.Core.Tools.Dithering;

public class DitheringAlgorithms
{
    public delegate void InFunc<TF, TS>(TF first, TS second);

    public static DitheringAlgorithms RawConversion = new(
        "Raw Conversion",
        RawConversionDithering.ApplyDithering);

    public static DitheringAlgorithms RandomConversion = new(
        "Random",
        RandomDithering.ApplyDithering);

    public static DitheringAlgorithms FloydSteinbergConversion = new(
        "Floyd Steinberg",
        FloydSteinbergDithering.ApplyDithering);

    public static DitheringAlgorithms AtkinsonConversion = new(
        "Atkinson",
        AtkinsonDithering.ApplyDithering);

    public static DitheringAlgorithms OrderConversion = new(
        "Order",
        OrderDithering.ApplyDithering);

    public DitheringAlgorithms(
        string name,
        InFunc<Bitmap, ColorDepth> applyDithering)
    {
        Name = name;
        ApplyDithering = applyDithering;
    }

    public string Name { get; }

    public InFunc<Bitmap, ColorDepth> ApplyDithering { get; }

    public static Lazy<IEnumerable<DitheringAlgorithms>> AllAlgorithms => new(
        () => typeof(DitheringAlgorithms)
            .GetFields(BindingFlags.Public | BindingFlags.Static)
            .Where(f => f.FieldType == typeof(DitheringAlgorithms))
            .Select(f => (DitheringAlgorithms)f.GetValue(null))
    );
}