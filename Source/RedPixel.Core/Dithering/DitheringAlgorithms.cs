﻿using System.Reflection;
using RedPixel.Core.Colors.ValueObjects;
using RedPixel.Core.Models;

namespace RedPixel.Core.Dithering;

public class DitheringAlgorithms
{
    public delegate void InFunc<TF, TS>(TF first, TS second);
    
    public string Name { get; }
    
    public InFunc<Bitmap, ColorDepth> ApplyDithering { get; }

    public DitheringAlgorithms(
        string name,
        InFunc<Bitmap, ColorDepth> applyDithering)
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
    
    public static DitheringAlgorithms OrderConversion = new DitheringAlgorithms(
        "Order",
        OrderDithering.ApplyDithering);
    
    public static Lazy<IEnumerable<DitheringAlgorithms>> AllAlgorithms => new(
        () => typeof(DitheringAlgorithms)
            .GetFields(BindingFlags.Public | BindingFlags.Static)
            .Where(f => f.FieldType == typeof(DitheringAlgorithms))
            .Select(f => (DitheringAlgorithms)f.GetValue(null))
    );
}