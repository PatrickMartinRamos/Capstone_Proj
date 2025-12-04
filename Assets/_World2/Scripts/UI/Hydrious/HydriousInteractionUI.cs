using UnityEngine;
using UnityEngine.UI;

namespace Stellarfarer
{
    [RequireComponent(typeof(HydriousUI))]
    [RequireComponent(typeof(Button))]
    public class HydriousInteractionUI : MonoBehaviour
    {
        [SerializeField] private HydriousUI _hydriousUI;
        [SerializeField] private Button _button;

        private void Awake()
        {
            if (_hydriousUI == null)
                _hydriousUI = GetComponent<HydriousUI>();

            if (_button == null)
                _button = GetComponent<Button>();

            _button.onClick.AddListener(() =>
            {
                if (Hydros7WorldManager.Instance.IsGamePlaying() || Hydros7WorldManager.Instance.IsDisplayingTutorial())
                {
                    if (CaptureManager.Instance.TryCapture(_hydriousUI))
                        _hydriousUI.Wait();
                }
            });
        }
    }
}