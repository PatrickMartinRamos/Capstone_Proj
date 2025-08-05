using System;
using UnityEngine;

namespace CapstoneProj.GridSystem
{
    public class Tile : MonoBehaviour
    {
        public event EventHandler OnExplode;

        [SerializeField] private Bomb _bomb;
        [SerializeField] private GridNavigator _gridNavigator;
        [SerializeField] private Vector2Int _tileCoordinates;

        public void SetTileCoordinates(Vector2Int coordinates)
            => _tileCoordinates = coordinates;

        public Vector2Int GetTileCoordinates()
            => _tileCoordinates;

        public Transform GetTileTransform()
            => transform;

        public void SetBomb(Bomb bomb)
            => _bomb = bomb;

        public Bomb GetBomb()
            => _bomb;

        public void ClearBomb()
            => _bomb = null;

        public bool HasBomb()
            => _bomb != null;

        public void SetGridNavigator(GridNavigator gridNavigator)
            => _gridNavigator = gridNavigator;

        public GridNavigator GetGridNavigator()
            => _gridNavigator;

        public void ClearGridNavigator()
            => _gridNavigator = null;

        public bool HasGridNavigator()
            => _gridNavigator != null;

        public void Explode()
        {
            OnExplode?.Invoke(this, EventArgs.Empty);
            GetBomb().DestroySelf();
        }
    }
}