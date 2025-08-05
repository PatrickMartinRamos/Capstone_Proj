using System;
using CapstoneProj.MiscSystem;
using UnityEngine;

namespace CapstoneProj.GridSystem
{
    public class GridNavigator : SingletonBehaviour<GridNavigator>
    {
        public event EventHandler OnDestroySelf;

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

        private void ResetParentTile()
        {
            _parentTile = null;
            transform.parent = null;
        }

        public void DestroySelf()
        {
            _parentTile.GetBomb().DestroySelf();
            _parentTile.ClearGridNavigator();

            ResetParentTile();
            OnDestroySelf?.Invoke(this, EventArgs.Empty);
        }

        public void DestroySelfContinuation()
        {
            BottomScreenBombSpawner bottomScreenBombSpawner = BottomScreenBombSpawner.Instance;
            
            Tile originalTopTile = bottomScreenBombSpawner.GetOriginalTopTile();
            originalTopTile.Explode();

            TileDetector.Instance.ResetInteraction();

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