namespace Stellarfarer
{
    [System.Flags]
    public enum Species
    {
        Piscyn = 1 << 0,
        Octomyn = 1 << 1,
        Chelon = 1 << 2,
        Carcinus = 1 << 3,
        Selar = 1 << 4,
        Valyn = 1 << 5
    }
}