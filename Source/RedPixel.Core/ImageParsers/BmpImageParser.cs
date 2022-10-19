using System.Diagnostics;
using System.Text;

namespace RedPixel.Core.ImageParsers;

public class BmpImageParser : IImageParser
{
    public Core.ImageFormat[] ImageFormats => new[] { Core.ImageFormat.Bmp, };

    public Bitmap.Bitmap Parse(Stream content)
    {
        throw new NotImplementedException();
    }

    public void SerializeToStream(Bitmap.Bitmap image, Stream stream)
    {
        // var img = image.GetSystemBitmap();
        // img.Save(stream, System.Drawing.Imaging.ImageFormat.Bmp);
        stream.Write(Encoding.ASCII.GetBytes("BM"));

        int size = 54 + image.Width * image.Height * 3;
        var sw = new Stopwatch();
        sw.Start();

        // TODO: Use Span<byte> to avoid allocations
        stream.Write(BitConverter.GetBytes(size), 0, 4);
        stream.Write(BitConverter.GetBytes(0), 0, 4);
        stream.Write(BitConverter.GetBytes(54), 0, 4);
        stream.Write(BitConverter.GetBytes(40), 0, 4);
        stream.Write(BitConverter.GetBytes(image.Width), 0, 4);
        stream.Write(BitConverter.GetBytes(image.Height), 0, 4);
        stream.Write(BitConverter.GetBytes((short)1), 0, 2);
        stream.Write(BitConverter.GetBytes((short)32), 0, 2);
        stream.Write(BitConverter.GetBytes(0), 0, 4);
        stream.Write(BitConverter.GetBytes(0), 0, 4);
        stream.Write(BitConverter.GetBytes(0), 0, 4);
        stream.Write(BitConverter.GetBytes(0), 0, 4);
        stream.Write(BitConverter.GetBytes(256*256*256), 0, 4);
        stream.Write(BitConverter.GetBytes(0), 0, 4);

        File.AppendAllText("time-log.txt", $"Writing header bmp: {sw.ElapsedMilliseconds}ms{Environment.NewLine}");

        for (int y = image.Height-1; y >= 0; y--)
        {
            for (int x = 0; x < image.Width; x++)
            {
                var pixel = image.GetPixel(x, y).ToRgb();
                stream.WriteByte(pixel.ThirdComponent);
                stream.WriteByte(pixel.SecondComponent);
                stream.WriteByte(pixel.FirstComponent);
                stream.WriteByte(1);
            }
        }

        sw.Stop();
        File.AppendAllText("time-log.txt", $"Writing body bmp: {sw.ElapsedMilliseconds}ms{Environment.NewLine}");
    }
}