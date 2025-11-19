using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Stellarfarer
{
    public class AzuliuzSpawnManager : SingletonBehaviour<AzuliuzSpawnManager>
    {
        [SerializeField] private RectTransform _azuliuzPrefab;

        private System.Random _rng;
        private List<AzuliuzUI> _azuliuzUIList = new List<AzuliuzUI>();

        private void InitializeRandom()
        {
            int seed = (int)((System.DateTime.Now.Ticks + Random.Range(0, 100000)) & 0x0000FFFF) ^ GetInstanceID();
            _rng = new System.Random(seed);
        }

        private int GetRandomInt(int maxValue)
            => _rng.Next(maxValue);

        protected override void Awake()
        {
            base.Awake();

            InitializeRandom();
        }

        public void Spawn()
        {
            HydriousUI hydriousUI = CaptureManager.Instance.GetHydriousUITarget();

            int spawnCount = hydriousUI.GetSpecies() switch
            {
                Species.Piscyn => 1,
                Species.Octomyn => 2,
                _ => 1
            };
            List<TileUI> reachableTileList = GridManager.Instance.GetReachableTileUIList();

            // Filter out occupied tiles first
            List<TileUI> availableTiles = reachableTileList
                .FindAll(tileUI => !tileUI.HasAzuliuzUI());

            if (availableTiles.Count == 0)
            {
                Debug.LogWarning("No available tiles to spawn Azuliuz!");
                return;
            }

            _azuliuzUIList.Clear();

            for (int i = 0; i < spawnCount && availableTiles.Count > 0; i++)
            {
                int randomIndex = GetRandomInt(availableTiles.Count);
                TileUI chosenTile = availableTiles[randomIndex];

                AzuliuzUI azuliuzUI = AzuliuzUI.SpawnAzuliuzUI(_azuliuzPrefab, chosenTile);
                _azuliuzUIList.Add(azuliuzUI);

                // Prevent spawning multiple Azuliuz on the same tile
                availableTiles.RemoveAt(randomIndex);
            }
        }

        public bool TryExtract()
            => _azuliuzUIList.All(azuliuzUI => azuliuzUI.GetTileUI().HasTargetUI());
    }
}