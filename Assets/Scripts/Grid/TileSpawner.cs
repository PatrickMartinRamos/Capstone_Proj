using System.Collections.Generic;
using CapstoneProj.MiscSystem;
using UnityEngine;

namespace CapstoneProj.GridSystem
{
    public class TileSpawner : SingletonBehaviour<TileSpawner>
    {
        [SerializeField] private Transform _tilePrefabTransform;
        [SerializeField] private Vector2 _boardDimensions = new Vector2(2, 2);
        [SerializeField] private (Color evenTileColor, Color oddTileColor) _tileColorArray = (Color.darkGray, Color.lightGray);
        private List<Tile> _tileList = new List<Tile>();

        protected override void Awake()
        {
            base.Awake();

            _tilePrefabTransform.gameObject.SetActive(false);

            SpawnGrid();
        }

        private void SpawnGrid()
        {
            foreach (Transform child in transform)
            {
                if (child == _tilePrefabTransform)
                    continue;

                Destroy(child.gameObject);
            }

            for (float y = _boardDimensions.y; y >= -_boardDimensions.y; y -= 0.5f)
            {
                for (float x = -_boardDimensions.x; x <= _boardDimensions.x; x += 0.5f)
                {
                    Transform tileTransform = Instantiate(_tilePrefabTransform, transform);
                    tileTransform.localPosition = new Vector3(x, y, 1f);
                    tileTransform.name = $"Tile_{x}_{y}";

                    if (tileTransform.TryGetComponent(out SpriteRenderer spriteRenderer))
                        spriteRenderer.color = (Mathf.Abs(x) + Mathf.Abs(y)) * 2f % 2f == 0f ? _tileColorArray.evenTileColor : _tileColorArray.oddTileColor;

                    if (tileTransform.TryGetComponent(out Tile tile))
                        _tileList.Add(tile);

                    tileTransform.gameObject.SetActive(true);
                }
            }
        }

        public List<Tile> GetTileList()
            => _tileList;
    }
}