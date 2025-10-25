namespace Stellarfarer
{
    public class HydriousDictSO : UnityEngine.ScriptableObject
    {
        [UnityEngine.SerializeField] private SerializedDictionary<Species, HydriousSO> _hydriousSODict = new();

        public System.Collections.Generic.IReadOnlyDictionary<Species, HydriousSO> HydriousSODict
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
                    UnityEngine.Debug.LogError($"Indexer only supports a single species flag, not multiple ({species}).");
                    return null;
                }

                return _hydriousSODict[species];
            }
            set
            {
                if (!species.IsSingleFlag())
                {
                    UnityEngine.Debug.LogError($"Indexer only supports a single species flag, not multiple ({species}).");
                    return;
                }

                _hydriousSODict[species] = value;
            }
        }

        public HydriousSO[] GetHydriousSOArrayBySpecies(Species species)
        {
            if (_hydriousSODict == null || _hydriousSODict.Count == 0)
                return null;

            System.Collections.Generic.List<HydriousSO> hydriousSOList = new();

            foreach (var speciesSingle in species.GetFlags())
            {
                if (_hydriousSODict.TryGetValue(speciesSingle, out var hydriousSO))
                    hydriousSOList.Add(hydriousSO);
                else
                {
                    UnityEngine.Debug.LogError($"[HydriousListSO] Species {speciesSingle} not found in dictionary.");
                    break;
                }
            }

            return hydriousSOList.ToArray();
        }
    }
}