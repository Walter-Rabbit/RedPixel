﻿using System.Reflection;
using RedPixel.Core.Models;

namespace RedPixel.Core.Tools.Filtering;

public class FilteringAlgorithms
{
    public delegate Bitmap InFunc<TF, TS>(TF first, TS second);

    public static readonly FilteringAlgorithms Threshold = new(
        "Threshold Filtering",
        "Threshold",
        "255",
        ThresholdFiltering.ApplyFiltering);

    public static readonly FilteringAlgorithms Otsu = new(
        "Otsu Filtering",
        "",
        "",
        OtsuFiltering.ApplyFiltering);

    public static readonly FilteringAlgorithms Median = new(
        "Median Filtering",
        "Core Radius",
        "18",
        MedianFiltering.ApplyFiltering);

    public static readonly FilteringAlgorithms Gaussian = new(
        "Gaussian Filtering",
        "Sigma",
        "6",
        GaussianFiltering.ApplyFiltering);

    public static readonly FilteringAlgorithms BoxBlur = new(
        "BoxBlur Filtering",
        "Core Radius",
        "18",
        BoxBlurFiltering.ApplyFiltering);

    public static readonly FilteringAlgorithms Sobel = new(
        "Sobel Filtering",
        "",
        "",
        SobelFiltering.ApplyFiltering);

    public static readonly FilteringAlgorithms Cas = new(
        "CAS Filtering",
        "Sharpness",
        "1",
        CasFiltering.ApplyFiltering);

    public FilteringAlgorithms(
        string name,
        string parameterName,
        string maxParameter,
        InFunc<Bitmap, float> applyFiltering)
    {
        Name = name;
        ParameterName = parameterName;
        MaxParameter = maxParameter;
        ApplyFiltering = applyFiltering;
    }

    public string Name { get; }
    public string ParameterName { get; }
    public string MaxParameter { get; }
    public InFunc<Bitmap, float> ApplyFiltering { get; }

    public static Lazy<IEnumerable<FilteringAlgorithms>> AllAlgorithms => new(
        () => typeof(FilteringAlgorithms)
            .GetFields(BindingFlags.Public | BindingFlags.Static)
            .Where(f => f.FieldType == typeof(FilteringAlgorithms))
            .Select(f => (FilteringAlgorithms)f.GetValue(null))
    );
}