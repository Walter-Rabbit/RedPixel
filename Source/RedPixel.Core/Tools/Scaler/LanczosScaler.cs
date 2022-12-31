using RedPixel.Core.Colors.ValueObjects;
using RedPixel.Core.Models;

namespace RedPixel.Core.Tools.Scaler;

public class LanczosScaler : FilterScaler
{
    protected override float Filter(float a, float point)
    {
        if (point < -WindowSize || point > WindowSize) return 0;
        point -= a;
        if (point == 0) return 1;
        return (float)Math.Round((WindowSize * Math.Sin(Math.PI * point) * Math.Sin(Math.PI * point / WindowSize) / (Math.PI * Math.PI * point * point)), 5);
    }

    protected override int WindowSize => 2;
}