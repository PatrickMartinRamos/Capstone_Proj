namespace Stellarfarer
{
    [System.Flags]
    public enum CleansingType
    {
        Plotting = 1 << 0,
        Midpoint = 1 << 1,
        Distance = 1 << 2
    }
}