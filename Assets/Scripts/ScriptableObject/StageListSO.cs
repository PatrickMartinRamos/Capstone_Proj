using System.Collections.Generic;
using UnityEngine;

namespace CapstoneProj.ScriptableObjectSystem
{
    public class StageListSO : ScriptableObject
    {
        public List<StageSO> StageSOList = new List<StageSO>();

        public StageSO GetStageSOByLevel(int level)
            => StageSOList.Find(stageSO => stageSO.StageID == level);
    }
}