using UnityEngine;

namespace Stellarfarer
{
    public class AzuliuzUI : MonoBehaviour
    {
        private RectTransform _rectTransform;
        private TileUI _tileUI;

        private void Awake()
            => _rectTransform = (RectTransform)transform;

        public void SetTileUI(TileUI tileUI)
        {
            _tileUI = tileUI;
            _tileUI.SetAzuliuzUI(this);

            _rectTransform.SetParent(tileUI.transform, true);
            _rectTransform.offsetMin = Vector2.zero;
            _rectTransform.offsetMax = Vector2.zero;
            _rectTransform.localScale = Vector3.one;
        }

        public TileUI GetTileUI()
            => _tileUI;

        public void DestroySelf()
        {
            _tileUI.ClearAzuliuzUI();
            Destroy(gameObject);
        }

        public static AzuliuzUI SpawnAzuliuzUI(
            RectTransform azuliuz,
            TileUI tile
        )
        {
            RectTransform azuliuzRectTransform = Instantiate(azuliuz);

            AzuliuzUI azuliuzUI = azuliuzRectTransform.GetComponent<AzuliuzUI>();
            azuliuzUI.SetTileUI(tile);
            return azuliuzUI;
        }
    }
}