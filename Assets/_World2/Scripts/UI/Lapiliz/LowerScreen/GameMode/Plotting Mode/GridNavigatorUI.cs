using System;
using UnityEngine;

namespace Stellarfarer
{
    public class GridNavigatorUI : MonoBehaviour
    {
        [SerializeField] private GridNavigatorSingleUI _left;
        [SerializeField] private GridNavigatorSingleUI _right;
        [SerializeField] private GridNavigatorSingleUI _down;
        [SerializeField] private GridNavigatorSingleUI _up;

        private GridManager _gridManager;

        private void Awake()
        {
            _left.OnInteracted
                += Left_OnInteracted;
            _right.OnInteracted
                += Right_OnInteracted;
            _down.OnInteracted
                += Down_OnInteracted;
            _up.OnInteracted
                += Up_OnInteracted;
        }

        private void Start()
        {
            if (GridManager.Instance != null)
                _gridManager = GridManager.Instance;
        }

        private void OnDestroy()
        {
            _left.OnInteracted
                -= Left_OnInteracted;
            _right.OnInteracted
                -= Right_OnInteracted;
            _down.OnInteracted
                -= Down_OnInteracted;
            _up.OnInteracted
                -= Up_OnInteracted;
        }

        private bool Left_OnInteracted()
            => CheckIfReachable(offSetX: -1);

        private bool Right_OnInteracted()
            => CheckIfReachable(offSetX: 1);

        private bool Down_OnInteracted()
            => CheckIfReachable(offSetY: -1);

        private bool Up_OnInteracted()
            => CheckIfReachable(offSetY: 1);

        private bool CheckIfReachable(float offSetX = 0, float offSetY = 0)
        {
            Vector2 gridCoordinates = _gridManager.GetGridCoordinates();
            gridCoordinates.x += offSetX;
            gridCoordinates.y += offSetY;

            if (_gridManager.IsTileUIReachableByGridCoordinates(gridCoordinates))
            {
                _gridManager.SetGridCoordinates(gridCoordinates);
                TileUI tileUI = _gridManager.GetTileUIByGridCoordinates(gridCoordinates);
                _gridManager.MoveTarget(tileUI);
                return true;
            }

            return false;
        }
    }
}