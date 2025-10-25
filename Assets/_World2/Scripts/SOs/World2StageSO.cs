namespace Stellarfarer
{
    public class World2StageSO : UnityEngine.ScriptableObject
    {
        [UnityEngine.SerializeField] private int _number;
        [UnityEngine.SerializeField] private CleansingType _cleansingType;
        [UnityEngine.SerializeField] private Species _species;
        [UnityEngine.SerializeField] private float _hydrionEnergyAmountRequired;
        [UnityEngine.SerializeField] private float _spawnTime;

        public int Number
            => _number;
        public CleansingType CleansingType
            => _cleansingType;
        public Species Species
            => _species;
        public float HydrionEnergyAmountRequired
            => _hydrionEnergyAmountRequired;
        public float SpawnTime
            => _spawnTime;

#if UNITY_EDITOR
        public void UpdateSO(
            int number,
            CleansingType cleansingType,
            Species species,
            float hydrionEnergyAmountRequired,
            float spawnTime)
        {
            if (!CompareNumber(number))
                _number = number;

            if (!CompareSpecies(species))
                _species = species;

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
        public bool CompareSpecies(Species species)
            => _species == species;
        public bool CompareHydrionEnergyAmountRequired(float hydrionEnergyAmountRequired)
            => _hydrionEnergyAmountRequired == hydrionEnergyAmountRequired;
        public bool CompareSpawnTime(float spawnTime)
            => _spawnTime == spawnTime;
    }
}