namespace RedPixel.Core.Tools.Scaler;

public class BCSplineScaler : FilterScaler
{
    private float B;
    private float C;

    public BCSplineScaler(float b, float c)
    {
        B = b;
        C = c;
    }

    protected override float Filter(float a, float point)
    {
        point -= a;
        point = Math.Abs(point);
        return point switch
        {
            < 1 => (12 - 9 * B - 6 * C) * (point * point * point) + (-18 + 12 * B + 6 * C) * (point * point) +
                   (6 - 2 * B),
            <= 2 => (-B - 6 * C) * (point * point * point) + (6 * B + 30 * C) * (point * point) +
                   (-12 * B - 48 * C) * point + (8 * B + 24 * C),
            _ => 0
        } / 6;
    }

    protected override int WindowSize => 2;
}