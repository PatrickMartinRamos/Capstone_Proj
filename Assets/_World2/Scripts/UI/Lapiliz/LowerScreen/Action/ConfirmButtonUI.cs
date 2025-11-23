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

        private void Awake()
        {
            if (_button == null)
                _button = GetComponent<Button>();

            _button.onClick.AddListener(() => LapilizManager.Instance.Confirm());
        }

        private void Start()
            => _button.image.ConfigureImageFromFullToVisibleSprite(
                full: _full,
                visible: _visible
            );
    }
}