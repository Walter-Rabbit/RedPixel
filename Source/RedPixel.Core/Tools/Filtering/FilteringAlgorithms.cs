using System.Drawing;
using System.Reflection;
using Bitmap = RedPixel.Core.Models.Bitmap;

namespace RedPixel.Core.Tools.Filtering;

public class FilteringAlgorithms
{
    public delegate Bitmap InFunc<T1, T2, T3, T4>(T1 first, T2 second, T3 third, T4 fourth);

    public static readonly FilteringAlgorithms Threshold = new(
        "Threshold",
        "Threshold",
        "255",
        ThresholdFiltering.ApplyFiltering);

    public static readonly FilteringAlgorithms Otsu = new(
        "Otsu",
        "",
        "0",
        OtsuFiltering.ApplyFiltering);

    public static readonly FilteringAlgorithms Median = new(
        "Median",
        "Core Radius",
        "18",
        MedianFiltering.ApplyFiltering);

    public static readonly FilteringAlgorithms Gaussian = new(
        "Gaussian",
        "Sigma",
        "6",
        GaussianFiltering.ApplyFiltering);

    public static readonly FilteringAlgorithms BoxBlur = new(
        "BoxBlur",
        "Core Radius",
        "18",
        BoxBlurFiltering.ApplyFiltering);

    public static readonly FilteringAlgorithms Sobel = new(
        "Sobel",
        "",
        "0",
        SobelFiltering.ApplyFiltering);

    public static readonly FilteringAlgorithms Cas = new(
        "CAS",
        "Sharpness",
        "1",
        CasFiltering.ApplyFiltering);

    public FilteringAlgorithms(
        string name,
        string parameterName,
        string maxParameter,
        InFunc<Bitmap, float, Point, Point> applyFiltering)
    {
        Name = name;
        ParameterName = parameterName;
        MaxParameter = maxParameter;
        ApplyFiltering = applyFiltering;
    }

    public string Name { get; }
    public string ParameterName { get; }
    public string MaxParameter { get; }
    public InFunc<Bitmap, float, Point, Point> ApplyFiltering { get; }

    public static Lazy<IEnumerable<FilteringAlgorithms>> AllAlgorithms => new(
        () => typeof(FilteringAlgorithms)
            .GetFields(BindingFlags.Public | BindingFlags.Static)
            .Where(f => f.FieldType == typeof(FilteringAlgorithms))
            .Select(f => (FilteringAlgorithms)f.GetValue(null))
    );
}