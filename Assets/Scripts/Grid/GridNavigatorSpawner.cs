using CapstoneProj.MiscSystem;
using UnityEngine;

namespace CapstoneProj.GridSystem
{
    public class GridNavigatorSpawner : SingletonBehaviour<GridNavigatorSpawner>
    {
        [SerializeField] private Transform _gridNavigatorPrefabTransform;
        private Tile _centerBottomTile;

        public void SetCenterBottomTile(Tile centerBottomTile)
            => _centerBottomTile = centerBottomTile;

        public void SpawnGridNavigator()
            => GridNavigator.SpawnGridNavigator(_gridNavigatorPrefabTransform, _centerBottomTile);
    }
}