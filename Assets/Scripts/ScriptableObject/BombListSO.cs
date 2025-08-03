using System.Collections.Generic;
using UnityEngine;

namespace CapstoneProj.ScriptableObjectSystem
{
    public class BombListSO : ScriptableObject
    {
        public List<BombSO> BombSOList = new List<BombSO>();
    }
}