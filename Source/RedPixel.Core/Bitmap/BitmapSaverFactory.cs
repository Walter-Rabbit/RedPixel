namespace RedPixel.Core.Bitmap;

using ImageFormat = System.Drawing.Imaging.ImageFormat;

public static class BitmapSaverFactory
{
    private static List<IBitmapSaver> Savers;

    static BitmapSaverFactory()
    {
        Savers = new List<IBitmapSaver>()
        {
            new BmpBitmapSaver()
        };
    }
    
    public static IBitmapSaver CreateSaver(ImageFormat format)
    {
        var parser = Savers.FirstOrDefault(p => Equals(p.ImageFormat, format));

        if (parser is null)
            throw new Exception("Unsupported image format");

        return parser;
    }
}