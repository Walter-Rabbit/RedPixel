namespace RedPixel.Core.Colors;

[Flags]
public enum ColorComponents
{
    None = 0,
    First = 1,
    Second = 2,
    Third = 4,
    All = First | Second | Third,
}