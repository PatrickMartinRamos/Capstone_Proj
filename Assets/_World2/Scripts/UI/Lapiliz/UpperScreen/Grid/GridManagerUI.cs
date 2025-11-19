using UnityEngine;
using UnityEngine.UI;

namespace Stellarfarer
{
    [RequireComponent(typeof(GridLayoutGroup))]
    public class GridManagerUI : MonoBehaviour
    {
        [SerializeField] private GridLayoutGroup _gridLayoutGroup;

        private RectTransform _rectTransform;
        private GridManager _gridManager;

        private void Awake()
        {
            _rectTransform = (RectTransform)transform;

            if (_gridLayoutGroup == null)
                _gridLayoutGroup = GetComponent<GridLayoutGroup>();
        }

        private void Start()
        {
            if (GridManager.Instance != null)
            {
                _gridManager = GridManager.Instance;

                _gridManager.OnGridSet
                    += GridManager_OnGridSet;
            }
        }

        private void OnDestroy()
        {
            if (_gridManager != null)
            {
                _gridManager.OnGridSet
                    -= GridManager_OnGridSet;
            }
        }

        private RectTransform GridManager_OnGridSet(float columnCount, float rowCount)
        {
            Rect rect = _rectTransform.rect;
            Vector2 cellSize = new Vector2(rect.width / columnCount, rect.height / rowCount);
            _gridLayoutGroup.cellSize = cellSize;
            return _rectTransform;
        }

        public void SetParent(Transform parent)
            => transform.SetParent(parent, true);
    }
}