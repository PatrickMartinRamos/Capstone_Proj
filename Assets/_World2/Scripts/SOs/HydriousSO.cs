using UnityEngine;

namespace Stellarfarer
{
    public class HydriousSO : ScriptableObject
    {
        [SerializeField] private Species _species;
        [SerializeField] private CleansingType _cleansingType;
        [SerializeField] private int _hydrionEnergyAmountGiven;
        [SerializeField] private Color _color;
        [SerializeField] private RectTransform _prefab;
        [SerializeField] private Sprite _mutatedSpriteLeft;
        [SerializeField] private Sprite _mutatedSpriteRight;
        [SerializeField] private Sprite _dizzySpriteLeft;
        [SerializeField] private Sprite _dizzySpriteRight;
        [SerializeField] private Sprite _cleansedSpriteLeft;
        [SerializeField] private Sprite _cleansedSpriteRight;
        [SerializeField] private Sprite _xraySprite;

        public Species Species
            => _species;
        public CleansingType CleansingType
            => _cleansingType;
        public int HydrionEnergyAmountGiven
            => _hydrionEnergyAmountGiven;
        public Color Color
            => _color;
        public RectTransform Prefab
            => _prefab;
        public Sprite MutatedSpriteLeft
            => _mutatedSpriteLeft;
        public Sprite MutatedSpriteRight
            => _mutatedSpriteRight;
        public Sprite DizzySpriteLeft
            => _dizzySpriteLeft;
        public Sprite DizzySpriteRight
            => _dizzySpriteRight;
        public Sprite CleansedSpriteLeft
            => _cleansedSpriteLeft;
        public Sprite CleansedSpriteRight
            => _cleansedSpriteRight;
        public Sprite XRaySprite
            => _xraySprite;

#if UNITY_EDITOR
        public void UpdateSO(
            Species species,
            CleansingType cleansingType,
            int hydrionEnergyAmountGiven,
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