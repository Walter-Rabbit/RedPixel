using System.Reflection;
using RedPixel.Core.Models;

namespace RedPixel.Core.Tools.Scaler;

public class ImageScaler
{
    public static ImageScaler Neighbour = new ImageScaler("Nearest Neighbour",
        (bitmap, w, h, param) =>  new NearestNeighbourScaler().Scale(bitmap, w, h));
    public static ImageScaler Bilinear = new ImageScaler("Bilinear", (bitmap, w, h, param) =>  new BilinearScaler().Scale(bitmap, w, h));
    public static ImageScaler Lanczos = new ImageScaler("Lanczos", (bitmap, w, h, param) =>  new LanczosScaler().Scale(bitmap, w, h));
    public static ImageScaler BCSpline = new ImageScaler("BC Splines", (bitmap, w, h, param) =>
    {
        var parameters = (param as float[]);
        return new BCSplineScaler(parameters[0], parameters[1]).Scale(bitmap, w, h);
    });

    public string Name { get; }
    public Func<Bitmap, int, int, object, Bitmap> Scaler { get; }

    public ImageScaler(string name, Func<Bitmap, int, int, object, Bitmap> scaler)
    {
        Name = name;
        Scaler = scaler;
    }

    public static Lazy<IEnumerable<ImageScaler>> All => new(
        () => typeof(ImageScaler)
            .GetFields(BindingFlags.Public | BindingFlags.Static)
            .Where(f => f.FieldType == typeof(ImageScaler))
            .Select(f => (ImageScaler)f.GetValue(null))
    );
}