using UnityEngine;

namespace Stellarfarer
{
    public class GridCoordinatesUI : MonoBehaviour
    {
        [SerializeField] private GridCoordinatesSingleUI _xCoordinate;
        [SerializeField] private GridCoordinatesSingleUI _yCoordinate;

        private GridManager _gridManager;

        private void Start()
        {
            if (GridManager.Instance != null)
            {
                _gridManager = GridManager.Instance;

                _gridManager.OnGridCoordinatesChanged
                    += GridManager_OnGridCoordinatesChanged;
            }
        }

        private void OnDestroy()
        {
            if (_gridManager != null)
            {
                _gridManager.OnGridCoordinatesChanged
                    -= GridManager_OnGridCoordinatesChanged;
            }
        }

        private void GridManager_OnGridCoordinatesChanged()
        {
            Vector2 gridCoordinates = _gridManager.GetGridCoordinates();

            _xCoordinate.SetCoordinateText(gridCoordinates.x);
            _yCoordinate.SetCoordinateText(gridCoordinates.y);
        }
    }
}