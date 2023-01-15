namespace RedPixel.Core.Tools.Scaler;

public class BilinearScaler : FilterScaler
{
    protected override float Filter(float a, float point)
    {
        return 1 - Math.Abs(point - a);
    }

    protected override int WindowSize => 1;
}