using UnityEngine;

namespace Stellarfarer
{
    public class World2_SpawnManager : SingletonBehaviour<World2_SpawnManager>
    {
        private readonly int SPAWN_COUNT_MAX = 10;

        public event System.Action OnNextSpawnChanged;

        [SerializeField] private HydriousDictSO _hydriousDictSO;
        [SerializeField] private UnityEngine.UI.Image _spawnArea;
        [SerializeField] private Bounds _spawnAreaBounds;

        private HydriousSO[] _spawnableHydriousSOArray;
        private readonly System.Collections.Generic.List<Hydrious> _spawnedHydriousList = new();
        private HydriousSO _nextSpawn;

        protected override void Awake()
        {
            base.Awake();

            if (_hydriousDictSO == null)
            {
                Debug.LogError($"{nameof(HydriousDictSO)} is null or uninitialized.");
                return;
            }

            if (_spawnArea == null)
            {
                Debug.LogError($"Spawn Area is null or uninitialized.");
                return;
            }
            else
            {
                Vector2 spawnAreaCenter = _spawnArea.GetActualVisibleSpriteCenter();
                Vector2 spawnAreaSize = _spawnArea.GetScaledVisibleSpriteSizeInUIUnits();
                _spawnAreaBounds = new Bounds(spawnAreaCenter, spawnAreaSize);
            }
        }

        public void InitializeSpawnManager(Species species)
        {
            _spawnableHydriousSOArray = _hydriousDictSO.GetHydriousSOArrayBySpecies(species);

            PreGameSpawn();
        }

        public void SpawnNextSpawn()
        {
            Spawn(_nextSpawn);
            GetNextSpawn();
        }

        public Color GetNextSpawnColor()
            => _nextSpawn.Color;

        private void PreGameSpawn()
        {
            int spawnCount = 3;

            for (int i = 0; i < spawnCount; i++)
            {
                var hydriousSO = GetRandomSpawn();

                Spawn(hydriousSO);
            }

            GetNextSpawn();
        }

        private void GetNextSpawn()
        {
            _nextSpawn = GetRandomSpawn();
            OnNextSpawnChanged?.Invoke();
        }

        private HydriousSO GetRandomSpawn()
        {
            int spawnableCount = _spawnableHydriousSOArray.Length;
            int randomIndex = Random.Range(0, spawnableCount);
            return _spawnableHydriousSOArray[randomIndex];
        }

        private void Spawn(HydriousSO hydriousSO)
        {
            if (_spawnedHydriousList.Count > SPAWN_COUNT_MAX)
            {
                World2_GameManager.Instance.LoseGame();
                return;
            }

            Vector2 spawnPosition = GetSpawnPosition(hydriousSO.Prefab);
            var hydrious = Hydrious.SpawnHydrious(hydriousSO, _spawnArea.rectTransform, spawnPosition);
            _spawnedHydriousList.Add(hydrious);
            _nextSpawn = null;
        }

        private Vector2 GetSpawnPosition(Transform hydriousTransform)
        {
            var hydriousRectTransform = hydriousTransform.GetComponent<RectTransform>();

            float hydriousLargestSize = System.MathF.Max(hydriousRectTransform.rect.size.x, hydriousRectTransform.rect.size.y);
            Vector2 spawnBounds = ((Vector2)_spawnAreaBounds.size / 2) + Vector2.one * hydriousLargestSize;

            Vector2 spawnPosition = Vector2.zero;
            int randomAxis = Random.Range(0, 2);
            int randomSide = Random.Range(0, 2);

            switch (randomAxis)
            {
                case 0:
                    float randomX = Random.Range(-spawnBounds.x, spawnBounds.x);
                    spawnPosition = new Vector2(
                        randomX,
                        randomSide switch
                        {
                            0 => -spawnBounds.y,
                            1 => spawnBounds.y,
                            _ => 0
                        });
                    break;
                case 1:
                    float randomY = Random.Range(-spawnBounds.y, spawnBounds.y);
                    spawnPosition = new Vector2(
                        randomSide switch
                        {
                            0 => -spawnBounds.x,
                            1 => spawnBounds.x,
                            _ => 0
                        },
                        randomY);
                    break;
            }

            var hydriousMovement = hydriousTransform.GetComponent<HydriousMovement>();
            hydriousMovement.SetMovementBounds(new Bounds(_spawnAreaBounds.center, spawnPosition));

            return spawnPosition + (Vector2)_spawnAreaBounds.center;
        }
    }
}