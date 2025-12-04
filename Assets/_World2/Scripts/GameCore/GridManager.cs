using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Stellarfarer
{
    public class GridManager : SingletonBehaviour<GridManager>
    {
        public event Func<float, float, RectTransform> OnGridSet;
        public event Action OnGridCoordinatesChanged;
        public event Action<bool> OnCartesianPlaneToggled;

        [SerializeField] private RectTransform _tilePrefab;
        [SerializeField] private RectTransform _targetPrefab;

        private TargetUI _targetUI;
        private List<TileUI> _tileUIList = new List<TileUI>();
        private List<TileUI> _reachableTileUIList = new List<TileUI>();
        private Vector2 _gridCoordinates = Vector2.zero;

        #region Target Management 
        public void SetTarget()
        {
            TileUI centerTileUI = GetCenterTileUI();
            _targetUI = SpawnTargetUI(centerTileUI);
        }

        private TargetUI SpawnTargetUI(TileUI tileUI)
            => TargetUI.SpawnTargetUI(_targetPrefab, tileUI);

        public void MoveTarget(TileUI tileUI)
            => _targetUI.SetTileUI(tileUI);

        public bool TryMark()
        {
            if (!_targetUI.HasTileUI())
                return false;

            TileUI tileUI = _targetUI.GetTileUI();

            if (tileUI.HasAzuliuzUI())
            {
                _targetUI.ClearTileUI();
                tileUI.Mark();
                SpawnTargetUI(tileUI);

                return true;
            }

            return false;
        }

        public Vector2 GetGridCoordinates()
            => _gridCoordinates;

        public void SetGridCoordinates(Vector2 gridCoordinates)
        {
            _gridCoordinates = gridCoordinates;
            OnGridCoordinatesChanged?.Invoke();
        }

        private void ResetGridCoordinates()
            => SetGridCoordinates(Vector2.zero);
        #endregion

        #region Grid Management
        protected override void Awake()
        {
            base.Awake();

            _tilePrefab.gameObject.SetActive(false);
        }

        public void SetGrid()
        {
            int columnCount = 13;
            int rowCount = 7;
            RectTransform parentRectTransform = OnGridSet?.Invoke(columnCount, rowCount);
            int columnCountHalf = Mathf.FloorToInt(columnCount / 2f);
            int rowCountHalf = Mathf.FloorToInt(rowCount / 2f);

            for (int y = rowCountHalf; y >= -rowCountHalf; y--)
            {
                for (int x = -columnCountHalf; x <= columnCountHalf; x++)
                {
                    RectTransform tile = Instantiate(_tilePrefab, parentRectTransform);
                    tile.gameObject.SetActive(true);
                    tile.name = $"Tile ({x}, {y})";

                    TileUI tileUI = tile.GetComponent<TileUI>();
                    tileUI.SetGridCoordinates(new Vector2(x, y));
                    bool isCenter = tileUI.IsCenterGrid();
                    Color tileColor = isCenter ? Color.darkGray : Color.lightGray;
                    float alpha = isCenter ? 0.1f : 0f;
                    tileUI.SetColor(tileColor, alpha);
                    _tileUIList.Add(tileUI);
                }
            }
        }

        public void ResetGridManager()
        {
            foreach (TileUI tileUI in _reachableTileUIList)
            {
                if (tileUI.HasAzuliuzUI())
                    tileUI.GetAzuliuzUI().DestroySelf();

                if (tileUI.HasTargetUI())
                    tileUI.GetTargetUI().DestroySelf();

                tileUI.Unmark();
            }

            ResetGridCoordinates();
        }

        public void SetReachableTileUIList(Image parent)
        {
            List<TileUI> visibleTileUIList = _tileUIList.FindAll(tileUI => tileUI.IsVisible(parent));
            _reachableTileUIList = new List<TileUI>(visibleTileUIList);
            _reachableTileUIList.RemoveAll(tileUI => !tileUI.IsReachableFromOrigin(visibleTileUIList));
        }

        public List<TileUI> GetReachableTileUIList()
            => _reachableTileUIList;

        public TileUI GetCenterTileUI()
            => GetTileUIByGridCoordinates(Vector2.zero);

        public TileUI GetTileUIByGridCoordinates(Vector2 gridCoordinates)
            => _tileUIList.Find(tileUI => tileUI.IsGridCoordinates(gridCoordinates));

        public TileUI GetReachableTileUIByGridCoordinates(Vector2 gridCoordinates)
            => _reachableTileUIList.Find(tileUI => tileUI.IsGridCoordinates(gridCoordinates));

        public bool IsTileUIReachableByGridCoordinates(Vector2 gridCoordinates)
            => GetReachableTileUIByGridCoordinates(gridCoordinates) != null;
        #endregion

        public void ToggleCartesianPlane(bool isToggled)
            => OnCartesianPlaneToggled?.Invoke(isToggled);
    }
}