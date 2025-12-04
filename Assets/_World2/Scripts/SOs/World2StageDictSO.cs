using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using UnityEngine;

namespace Stellarfarer
{
    public class World2StageDictSO : ScriptableObject
    {
        [SerializeField] private SerializedDictionary<int, World2StageSO> _world2StageSODict = new();

        public IReadOnlyDictionary<int, World2StageSO> World2StageSODict
            => _world2StageSODict;

#if UNITY_EDITOR
        public void TryAdd(World2StageSO world2StageSO)
        {
            int number = world2StageSO.Number;

            if (!_world2StageSODict.TryAdd(number, world2StageSO))
                _world2StageSODict[number] = world2StageSO;
        }
#endif

        public World2StageSO this[int id]
        {
            get { return _world2StageSODict[id]; }
            // set { _world2StageSODict[id] = value; }
        }

        public int Count()
            => _world2StageSODict.Count;
    }
}