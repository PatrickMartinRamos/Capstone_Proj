using UnityEngine;

namespace Stellarfarer
{
    public class HydriousSpawnManager : SingletonBehaviour<HydriousSpawnManager>
    {
        private readonly int SPAWN_COUNT_MAX = 10;

        public event System.Action OnNextSpawnChanged;

        [SerializeField] private HydriousDictSO _hydriousDictSO;
        [SerializeField] private UnityEngine.UI.Image _spawnArea;
        [SerializeField] private Sprite _full;
        [SerializeField] private Sprite _visible;

        private HydriousSO[] _spawnableHydriousUISOArray;
        private readonly System.Collections.Generic.List<HydriousUI> _spawnedHydriousUIList = new();
        private HydriousSO _nextSpawn;
        private System.Random _rng;

        private void InitializeRandom()
        {
            int seed = (int)((System.DateTime.Now.Ticks + Random.Range(0, 100000)) & 0x0000FFFF) ^ GetInstanceID();
            _rng = new System.Random(seed);
        }

        private int GetRandomInt(int maxValue)
            => _rng.Next(maxValue);

        private float GetRandomFloat(float minValue, float maxValue)
        {
            float randomValue = (float)_rng.NextDouble();
            return Mathf.Lerp(minValue, maxValue, randomValue);
        }

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

            InitializeRandom();
        }

        private void Start()
            => _spawnArea.ConfigureImageFromFullToVisibleSprite(
                    full: _full,
                    visible: _visible
                );

        public void InitializeSpawnManager(CleansingType cleansingType)
        {
            _spawnableHydriousUISOArray = _hydriousDictSO.GetHydriousSOArrayByCleansingType(cleansingType);

            PreGameSpawn();
        }

        public void SpawnNextSpawn()
        {
            Spawn(_nextSpawn);
            GetNextSpawn();
        }

        public Color GetNextSpawnColor()
            => _nextSpawn.Color;

        public RectTransform GetSpawnAreaRectTransform()
            => _spawnArea.rectTransform;

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
            int spawnableCount = _spawnableHydriousUISOArray.Length;
            int randomIndex = GetRandomInt(spawnableCount);
            return _spawnableHydriousUISOArray[randomIndex];
        }

        private void Spawn(HydriousSO hydriousSO)
        {
            if (_spawnedHydriousUIList.Count > SPAWN_COUNT_MAX)
            {
                Hydros7GameManager.Instance.LoseGame();
                return;
            }

            Vector2 spawnPosition = GetSpawnPosition(hydriousSO.Prefab);
            HydriousUI hydriousUI = HydriousUI.SpawnHydriousUI(hydriousSO, _spawnArea.rectTransform, spawnPosition);
            _spawnedHydriousUIList.Add(hydriousUI);
            _nextSpawn = null;
        }

        private Vector2 GetSpawnPosition(RectTransform hydriousRectTransform)
        {
            Vector3[] worldCorners = new Vector3[4];
            _spawnArea.rectTransform.GetWorldCorners(worldCorners);
            Vector2 hydriousSize = hydriousRectTransform.rect.size;

            Vector2 posMin = (Vector2)worldCorners[0] - hydriousSize;
            Vector2 posMax = (Vector2)worldCorners[2] + hydriousSize;

            int randomAxis = GetRandomInt(2);
            int randomSide = GetRandomInt(2);

            Vector2 randomSpawnPosition;

            switch (randomAxis)
            {
                case 0:
                    float randomPosX = GetRandomFloat(posMin.x, posMax.x);
                    randomSpawnPosition = new Vector2(
                        randomPosX,
                        randomSide switch
                        {
                            0 => posMin.y,
                            1 => posMax.y,
                            _ => posMin.y
                        });
                    break;
                case 1:
                    float randomPosY = GetRandomFloat(posMin.y, posMax.y);
                    randomSpawnPosition = new Vector2(
                        randomSide switch
                        {
                            0 => posMin.x,
                            1 => posMax.x,
                            _ => posMin.x
                        },
                        randomPosY);
                    break;
                default:
                    randomSpawnPosition = posMin;
                    break;
            }

            return randomSpawnPosition;
        }
    }
}