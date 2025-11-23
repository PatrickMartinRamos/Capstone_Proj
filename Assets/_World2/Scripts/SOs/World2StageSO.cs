using UnityEngine;

namespace Stellarfarer
{
    public class World2StageSO : ScriptableObject
    {
        [SerializeField] private int _number;
        [SerializeField] private CleansingType _cleansingType;
        [SerializeField] private int _hydrionEnergyAmountRequired;
        [SerializeField] private float _spawnTime;

        public int Number
            => _number;
        public CleansingType CleansingType
            => _cleansingType;
        public int HydrionEnergyAmountRequired
            => _hydrionEnergyAmountRequired;
        public float SpawnTime
            => _spawnTime;

#if UNITY_EDITOR
        public void UpdateSO(
            int number,
            CleansingType cleansingType,
            int hydrionEnergyAmountRequired,
            float spawnTime)
        {
            if (!CompareNumber(number))
                _number = number;

            if (!CompareCleansingType(cleansingType))
                _cleansingType = cleansingType;

            if (!CompareHydrionEnergyAmountRequired(hydrionEnergyAmountRequired))
                _hydrionEnergyAmountRequired = hydrionEnergyAmountRequired;

            if (!CompareSpawnTime(spawnTime))
                _spawnTime = spawnTime;
        }
#endif

        public bool CompareNumber(int number)
            => _number == number;
        public bool CompareCleansingType(CleansingType cleansingType)
            => _cleansingType == cleansingType;
        public bool CompareHydrionEnergyAmountRequired(float hydrionEnergyAmountRequired)
            => _hydrionEnergyAmountRequired == hydrionEnergyAmountRequired;
        public bool CompareSpawnTime(float spawnTime)
            => _spawnTime == spawnTime;
    }
}