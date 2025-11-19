using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using UnityEngine;

namespace Stellarfarer
{
    public class HydriousDictSO : ScriptableObject
    {
        [SerializeField] private SerializedDictionary<Species, HydriousSO> _hydriousSODict = new();

        public IReadOnlyDictionary<Species, HydriousSO> HydriousSODict
            => _hydriousSODict;

#if UNITY_EDITOR
        public void TryAdd(HydriousSO hydriousSO)
        {
            Species species = hydriousSO.Species;

            if (!_hydriousSODict.TryAdd(species, hydriousSO))
                _hydriousSODict[species] = hydriousSO;
        }
#endif

        public HydriousSO this[Species species]
        {
            get
            {
                if (!species.IsSingleFlag())
                {
                    Debug.LogError($"Indexer only supports a single species flag, not multiple ({species}).");
                    return null;
                }

                return _hydriousSODict[species];
            }
            set
            {
                if (!species.IsSingleFlag())
                {
                    Debug.LogError($"Indexer only supports a single species flag, not multiple ({species}).");
                    return;
                }

                _hydriousSODict[species] = value;
            }
        }

        public HydriousSO[] GetHydriousSOArrayByCleansingType(CleansingType cleansingType)
        {
            if (_hydriousSODict == null || _hydriousSODict.Count == 0)
                return null;

            List<HydriousSO> hydriousSOList = new();

            foreach (var cleaningTypeSingle in cleansingType.GetFlags())
            {
                foreach (HydriousSO hydriousSO in _hydriousSODict.Values)
                {
                    if (hydriousSO.CompareCleansingType(cleaningTypeSingle))
                        hydriousSOList.Add(hydriousSO);
                }
            }

            return hydriousSOList.ToArray();
        }

        public HydriousSO[] GetHydriousSOArrayBySpecies(Species species)
        {
            if (_hydriousSODict == null || _hydriousSODict.Count == 0)
                return null;

            List<HydriousSO> hydriousSOList = new();

            foreach (var speciesSingle in species.GetFlags())
            {
                if (_hydriousSODict.TryGetValue(speciesSingle, out var hydriousSO))
                    hydriousSOList.Add(hydriousSO);
                else
                {
                    Debug.LogError($"[HydriousDictSO] Cleansing Type {speciesSingle} not found in dictionary.");
                    break;
                }
            }

            return hydriousSOList.ToArray();
        }
    }
}