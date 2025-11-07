using UnityEngine;

namespace Stellarfarer
{
    public class HydriousSO : ScriptableObject
    {
        [SerializeField] private Species _species;
        [SerializeField] private CleansingType _cleansingType;
        [SerializeField] private float _hydrionEnergyAmountGiven;
        [SerializeField] private Color _color;
        [SerializeField] private Transform _prefab;

        public Species Species
            => _species;
        public CleansingType CleansingType
            => _cleansingType;
        public float HydrionEnergyAmountGiven
            => _hydrionEnergyAmountGiven;
        public Color Color
            => _color;
        public Transform Prefab
            => _prefab;

#if UNITY_EDITOR
        public void UpdateSO(
            Species species,
            CleansingType cleansingType,
            float hydrionEnergyAmountGiven,
            Color color)
        {
            if (!CompareSpecies(species))
                _species = species;

            if (!CompareCleansingType(cleansingType))
                _cleansingType = cleansingType;

            if (!CompareHydrionEnergyAmountGiven(hydrionEnergyAmountGiven))
                _hydrionEnergyAmountGiven = hydrionEnergyAmountGiven;

            if (!CompareColor(color))
                _color = color;
        }
#endif

        public bool CompareSpecies(Species species)
            => _species == species;
        public bool CompareCleansingType(CleansingType cleansingType)
            => _cleansingType == cleansingType;
        public bool CompareHydrionEnergyAmountGiven(float hydrionEnergyAmountGiven)
            => _hydrionEnergyAmountGiven == hydrionEnergyAmountGiven;
        public bool CompareColor(Color color)
            => _color == color;
    }
}