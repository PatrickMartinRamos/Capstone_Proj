using System.Collections.Generic;
using CapstoneProj.MiscSystem;
using UnityEngine;

namespace CapstoneProj.GridSystem
{
    public class BottomScreenTileSpawner : SingletonBehaviour<BottomScreenTileSpawner>
    {
        [SerializeField] private Transform _bottomTilePrefabTransform;
        private List<Tile> _bottomTileList = new List<Tile>();

        protected override void Awake()
        {
            base.Awake();

            _bottomTilePrefabTransform.gameObject.SetActive(false); // Ensure the prefab is not active in the scene.
        }

        public void SpawnGrid(List<Tile> topTileList)
        {
            foreach (Tile topTile in topTileList)
            {
                Transform bottomTileTransform = Instantiate(_bottomTilePrefabTransform, transform);
                bottomTileTransform.localScale = topTile.GetTileTransform().localScale;
                bottomTileTransform.localPosition = topTile.GetTileTransform().localPosition;
                bottomTileTransform.name = topTile.name.Replace("Top", "Bottom");
                bottomTileTransform.gameObject.SetActive(true);

                if (bottomTileTransform.TryGetComponent(out Tile bottomTile))
                {
                    Vector2Int tileCoordinates = topTile.GetTileCoordinates();
                    bottomTile.SetTileCoordinates(tileCoordinates);
                    _bottomTileList.Add(bottomTile);

                    if (tileCoordinates == Vector2Int.zero)
                        GridNavigatorSpawner.Instance.SetCenterBottomTile(bottomTile);
                }
            }
        }

        public List<Tile> GetBottomTileList()
            => _bottomTileList;
    }
}