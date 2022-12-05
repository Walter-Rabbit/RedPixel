using System.Reflection;
using RedPixel.Core.Models;

namespace RedPixel.Core.Tools.Filtering;

public class FilteringAlgorithms
{
    public delegate void InFunc<TF, TS>(TF first, TS second);

    public static FilteringAlgorithms Threshold = new(
        "Threshold Filtering",
        ThresholdFiltering.ApplyFiltering);

    public static FilteringAlgorithms Otsu = new(
        "Otsu Filtering",
        OtsuFiltering.ApplyFiltering);

    public static FilteringAlgorithms Median = new(
        "Median Filtering",
        MedianFiltering.ApplyFiltering);

    public static FilteringAlgorithms Gaussian = new(
        "Gaussian Filtering",
        GaussianFiltering.ApplyFiltering);

    public static FilteringAlgorithms BoxBlur = new(
        "BoxBlur Filtering",
        BoxBlurFiltering.ApplyFiltering);

    public static FilteringAlgorithms Sobel = new(
        "Sobel Filtering",
        SobelFiltering.ApplyFiltering);

    public static FilteringAlgorithms Cas = new(
        "CAS Filtering",
        CasFiltering.ApplyFiltering);

    public FilteringAlgorithms(
        string name,
        InFunc<Bitmap, float> applyFiltering)
    {
        Name = name;
        ApplyFiltering = applyFiltering;
    }

    public string Name { get; }

    public InFunc<Bitmap, float> ApplyFiltering { get; }

    public static Lazy<IEnumerable<FilteringAlgorithms>> AllAlgorithms => new(
        () => typeof(FilteringAlgorithms)
            .GetFields(BindingFlags.Public | BindingFlags.Static)
            .Where(f => f.FieldType == typeof(FilteringAlgorithms))
            .Select(f => (FilteringAlgorithms)f.GetValue(null))
    );
}