using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Stellarfarer
{
    public class TargetUI : MonoBehaviour
    {
        [SerializeField] private Image _visual;

        private RectTransform _rectTransform;
        private TileUI _tileUI;
        private Sequence _sequence;

        private void Awake()
        {
            _rectTransform = (RectTransform)transform;

            Color visualColor = _visual.color;
            visualColor.a = 0f;
            _visual.color = visualColor;

            _sequence = DOTween.Sequence();
            _sequence.Append(_visual.DOFade(1f, 1f));
            _sequence.Append(_visual.DOFade(0.75f, 1f));
            _sequence.SetLoops(-1, LoopType.Restart);
            _sequence.SetLink(gameObject, LinkBehaviour.KillOnDestroy);
        }

        public bool HasTileUI()
            => _tileUI != null;

        public void SetTileUI(TileUI tileUI)
        {
            ClearTileUI();

            _tileUI = tileUI;
            _tileUI.SetTargetUI(this);

            if (_tileUI.HasAzuliuzUI())
            {
                _tileUI.SetAlpha(0.1f);

                if (!_tileUI.IsMarked())
                    Hydros7WorldManager.Instance.TryDisplayTutorial(TutorialManager.Instance.TryDisplaySwitchToTab1Tutorial);
            }

            _rectTransform.SetParent(tileUI.transform, true);
            _rectTransform.offsetMin = Vector2.zero;
            _rectTransform.offsetMax = Vector2.zero;
            _rectTransform.localScale = Vector3.one;
        }

        public void ClearTileUI()
        {
            if (!HasTileUI())
                return;

            _tileUI.ResetAlpha();
            _tileUI.ClearTargetUI();
            _tileUI = null;
        }

        public TileUI GetTileUI()
            => _tileUI;

        public void DestroySelf()
        {
            ClearTileUI();

            Destroy(gameObject);
        }

        public static TargetUI SpawnTargetUI(
            RectTransform azuliuz,
            TileUI tile
        )
        {
            RectTransform azuliuzRectTransform = Instantiate(azuliuz);

            TargetUI targetUI = azuliuzRectTransform.GetComponent<TargetUI>();
            targetUI.SetTileUI(tile);
            return targetUI;
        }
    }
}