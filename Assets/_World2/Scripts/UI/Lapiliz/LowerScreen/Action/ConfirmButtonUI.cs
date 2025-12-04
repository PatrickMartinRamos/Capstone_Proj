using UnityEngine;
using UnityEngine.UI;

namespace Stellarfarer
{
    [RequireComponent(typeof(Button))]
    public class ConfirmButtonUI : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private Sprite _full;
        [SerializeField] private Sprite _visible;

        private LapilizManager _lapilizManager;

        private void Awake()
        {
            if (_button == null)
                _button = GetComponent<Button>();

            _button.onClick.AddListener(() =>
            {
                if (Hydros7WorldManager.Instance.IsGamePlaying() ||
                (Hydros7WorldManager.Instance.IsDisplayingTutorial() &&
                (TutorialManager.Instance.IsDisplayingConfirmTutorial() ||
                TutorialManager.Instance.IsWaiting())))
                {
                    Hydros7WorldManager.Instance.TryDisplayTutorial(TutorialManager.Instance.Wait);
                    LapilizManager.Instance.Confirm();
                }
            });
        }

        private void Start()
        {
            _button.image.ConfigureImageFromFullToVisibleSprite(
                full: _full,
                visible: _visible
            );

            if (LapilizManager.Instance != null)
            {
                _lapilizManager = LapilizManager.Instance;

                _lapilizManager.OnConfirmButtonRectGot
                    += LapilizManager_OnConfirmButtonRectGot;
            }
        }

        private void OnDestroy()
        {
            if (_lapilizManager != null)
            {
                _lapilizManager.OnConfirmButtonRectGot
                    -= LapilizManager_OnConfirmButtonRectGot;
            }
        }

        private Rect LapilizManager_OnConfirmButtonRectGot()
        {
            RectTransform toggleRectTransform = _button.image.rectTransform;
            return new Rect(toggleRectTransform.position, toggleRectTransform.sizeDelta);
        }
    }
}