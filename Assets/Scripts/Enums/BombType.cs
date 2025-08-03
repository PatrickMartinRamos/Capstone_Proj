namespace CapstoneProj.EnumSystem
{
    [System.Flags]
    public enum BombType
    {
        None = 0,
        Everything = 1 << 0,
        GrayBomb = 1 << 1,
        BlueBomb = 1 << 2,
        GreenBomb = 1 << 3,
        YellowBomb = 1 << 4,
        VioletBomb = 1 << 5,
        RainbowBomb = 1 << 6
    }
}