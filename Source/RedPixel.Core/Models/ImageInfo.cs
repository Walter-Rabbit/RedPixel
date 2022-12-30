namespace RedPixel.Core.Models;

public record ImageInfo(
    int Width, 
    int Height, 
    int BytesForColor, 
    ColorTypes ColorType, 
    int CompressionMethod,
    int FilteringMethod,
    int Interlacing);