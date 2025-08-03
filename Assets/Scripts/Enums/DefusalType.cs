namespace CapstoneProj.EnumSystem
{
    [System.Flags]
    public enum DefusalType
    {
        None = 0,
        Everything = 1 << 0,
        Plotting = 1 << 1,
        Midpoint = 1 << 2,
        Distance = 1 << 3,
    }
}