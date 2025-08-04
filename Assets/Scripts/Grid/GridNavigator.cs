using CapstoneProj.MiscSystem;
using UnityEngine;

namespace CapstoneProj.GridSystem
{
    public class GridNavigator : SingletonBehaviour<GridNavigator>
    {
        private Tile _parentTile;

        public void SetParentTile(Tile parentTile)
        {
            if (_parentTile != null)
                _parentTile.ClearGridNavigator();

            if (parentTile.HasGridNavigator())
                Debug.LogError("Tile already has a Grid Navigator"); 

            _parentTile = parentTile;
            parentTile.SetGridNavigator(this);
            transform.parent = parentTile.GetTileTransform();
            transform.localPosition = Vector3.zero;
        }

        public Tile GetParentTile()
            => _parentTile;

        public void DestroySelf()
        {
            _parentTile.ClearGridNavigator();

            Destroy(gameObject);
        }

        public static GridNavigator SpawnGridNavigator(Transform gridNavigatorPrefabTransform, Tile parentTile)
        {
            Transform gridNavigatorTransform = Instantiate(gridNavigatorPrefabTransform, parentTile.GetTileTransform());

            if (gridNavigatorTransform.TryGetComponent(out GridNavigator gridNavigator))
            {
                gridNavigator.SetParentTile(parentTile);
                return gridNavigator;
            }

            return null;
        }
    }
}