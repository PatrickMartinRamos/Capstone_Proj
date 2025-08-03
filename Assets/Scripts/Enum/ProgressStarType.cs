using System;

namespace CapstoneProj.EnumSystem
{
    public enum ProgressStarType
    {
        Star0,
        Star1,
        Star2,
        Star3
    }

    [Serializable]
    public struct ProgressStarTypeBombSpawnInterval
    {
        public ProgressStarType progressStarType;
        public float BombSpawnInterval;
    }
}