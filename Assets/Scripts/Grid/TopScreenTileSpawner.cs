using System.Collections.Generic;
using CapstoneProj.MiscSystem;
using UnityEngine;

namespace CapstoneProj.GridSystem
{
    public class TopScreenTileSpawner : SingletonBehaviour<TopScreenTileSpawner>
    {
        [SerializeField] private Transform _topTilePrefabTransform;
        [SerializeField] private Vector2 _boardDimensions = new Vector2(2, 2);
        private readonly List<Tile> _topTileList = new List<Tile>();

        protected override void Awake()
        {
            base.Awake();

            _topTilePrefabTransform.gameObject.SetActive(false);
        }

        private void Start()
            => SpawnGrid();

        private void SpawnGrid()
        {
            foreach (Transform child in transform)
            {
                if (child == _topTilePrefabTransform)
                    continue;

                Destroy(child.gameObject);
            }

            for (float y = _boardDimensions.y; y >= -_boardDimensions.y; y -= 0.5f)
            {
                for (float x = -_boardDimensions.x; x <= _boardDimensions.x; x += 0.5f)
                {
                    Transform topTileTransform = Instantiate(_topTilePrefabTransform, transform);
                    topTileTransform.localPosition = new Vector3(x, y, 1f);
                    topTileTransform.name = $"TopTile_{x}_{y}";
                    topTileTransform.gameObject.SetActive(true);

                    if (topTileTransform.TryGetComponent(out Tile tile))
                        _topTileList.Add(tile);
                }
            }

            TopScreenBombSpawner.Instance.SpawnInitialBombs(_topTileList);
            BombSpawnTimer.Instance.ResetCountdown();
            InputToTileDetector.Instance.SetGridReady(true);
            BottomScreenTileSpawner.Instance.SpawnGrid(_topTileList);
        }

        public List<Tile> GetTopTileList()
            => _topTileList;
    }
}