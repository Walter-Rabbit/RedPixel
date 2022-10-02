namespace RedPixel.Core.Tools;

public static class HeaderMatchFuncFactory
{
    public static Func<Stream, bool> Create(params byte[][] headers)
    {
        return (content) =>
        {
            return headers.Any(header =>
            {
                var formatHeader = new byte[header.Length];
                content.Read(formatHeader);
                content.Seek(0, SeekOrigin.Begin);
                return header.SequenceEqual(formatHeader);
            });
        };
    }
}