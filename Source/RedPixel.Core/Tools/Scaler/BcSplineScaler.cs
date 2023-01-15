namespace RedPixel.Core.Tools.Scaler;

public class BcSplineScaler : FilterScaler
{
    private readonly float _b;
    private readonly float _c;

    public BcSplineScaler(float b, float c)
    {
        _b = b;
        _c = c;
    }

    protected override float Filter(float a, float point)
    {
        point -= a;
        point = Math.Abs(point);
        return point switch
        {
            < 1 => (12 - 9 * _b - 6 * _c) * (point * point * point) + (-18 + 12 * _b + 6 * _c) * (point * point) +
                   (6 - 2 * _b),
            <= 2 => (-_b - 6 * _c) * (point * point * point) + (6 * _b + 30 * _c) * (point * point) +
                    (-12 * _b - 48 * _c) * point + (8 * _b + 24 * _c),
            _ => 0
        } / 6;
    }

    protected override int WindowSize => 2;
}