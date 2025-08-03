using System.Collections.Generic;
using CapstoneProj.EnumSystem;
using UnityEngine;

namespace CapstoneProj.ScriptableObjectSystem
{
    public class StageSO : ScriptableObject
    {
        public int StageID;
        public WorldSO WorldSO;
        public string StageName;
        public float StageDuration;
        public int InitialBombCount;
        public DefusalType DefusalType;
        public BombType BombType;
        public List<ProgressStarTypeBombSpawnInterval> ProgressStarTypeBombSpawnIntervalList = new List<ProgressStarTypeBombSpawnInterval>();
        public int RequiredPoints;
    }
}