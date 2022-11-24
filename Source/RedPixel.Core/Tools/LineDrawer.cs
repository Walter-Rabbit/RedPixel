using RedPixel.Core.Colors.ValueObjects;
using RedPixel.Core.Models;

namespace RedPixel.Core.Tools;

public static class LineDrawer
{
    private static void DrawPoint(Bitmap bitmap, int x, int y, float alpha, Color color)
    {
        if (x < 0 || x >= bitmap.Width || y < 0 || y >= bitmap.Height)
            return;

        alpha = alpha < 0 ? 0 : alpha;

        var currentColor = bitmap.Matrix[y, x];
        var newColor = new Color(
            color.FirstComponent * alpha + currentColor.FirstComponent * (1 - alpha),
            color.SecondComponent * alpha + currentColor.SecondComponent * (1 - alpha),
            color.ThirdComponent * alpha + currentColor.ThirdComponent * (1 - alpha)
        );

        bitmap.Matrix[y, x] = newColor;
    }

    private static int IntPart(float number) => (int)number;

    private static void Plot(Bitmap bitmap, int x, int y, float alpha, Color color, bool steep)
    {
        if (steep)
        {
            DrawPoint(bitmap, y ,x, alpha, color);
        }
        else
        {
            DrawPoint(bitmap, x, y, alpha, color);
        }
    }

    private static float Distance(int x0, int y0, int x1, int y1)
    {
        return MathF.Sqrt((x0 - x1) * (x0 - x1) + (y0 - y1) * (y0 - y1));
    }

    public static void DrawLine(this Bitmap bitmap, int xStart, int yStart, int xEnd, int yEnd, float opacity, Color color, int thickness)
    {
        var steep = Math.Abs(yEnd - yStart) > Math.Abs(xEnd - xStart);

        if (steep)
        {
            (xStart, yStart) = (yStart, xStart);
            (xEnd, yEnd) = (yEnd, xEnd);
        }

        if (xStart > xEnd)
        {
            (xStart, xEnd) = (xEnd, xStart);
            (yStart, yEnd) = (yEnd, yStart);
        }

        var dx = xEnd - xStart;
        var dy = yEnd - yStart;
        var gradient = (float)dy / dx;
        float y = yStart;

        for (int x = xStart; x <= xEnd; x++)
        {
            for (int plotY = IntPart(y - (thickness - 1) / 2);
                 plotY <= IntPart(y - (thickness - 1) / 2 + thickness);
                 plotY++)
            {
                var intensity = MathF.Min(1.0f, (thickness + 1.0f) / 2.0f - MathF.Abs(y - plotY));
                Plot(bitmap, x, plotY, intensity*opacity, color, steep);
            }

            y += gradient;
        }

        for (int plotX = xStart - thickness / 2; plotX < xStart; plotX++)
        {
            y = yStart + gradient * (plotX - xStart);
            for (int plotY = (int)(y - (thickness - 1) / 2.0);
                 plotY <= (int)(y - (thickness - 1) / 2.0 + thickness);
                 plotY++)
            {
                var intensity = MathF.Min(1.0f, (thickness + 0.5f) / 2.0f - Distance(plotX, plotY, xStart, yStart));
                Plot(bitmap, plotX, plotY, intensity*opacity, color, steep);
            }
        }

        for (var plotX = xEnd + 1; plotX <= xEnd + thickness / 2; plotX++) {
            y = yStart + gradient * (plotX - xStart);
            for (int plotY = (int)(y - (thickness - 1) / 2.0); plotY <= (int)(y - (thickness - 1) / 2.0 + thickness); plotY++)
            {
                var intensity = MathF.Min(1.0f, (thickness + 0.5f) / 2.0f - Distance(plotX, plotY, xEnd, yEnd));
                Plot(bitmap, plotX, plotY, intensity*opacity, color, steep);
            }
        }
    }
}