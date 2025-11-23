using UnityEngine;
using UnityEngine.UI;

namespace Stellarfarer
{
    [RequireComponent(typeof(Button))]
    public class PauseButtonUI : MonoBehaviour
    {
        [SerializeField] Button _button;

        private void Awake()
        {
            if (_button == null)
                _button = GetComponent<Button>();

            _button.onClick.AddListener(() => Hydros7GameManager.Instance.ToggleGamePause());
        }
    }
}