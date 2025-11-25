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

            _button.onClick.AddListener(() =>
            {
                Hydros7WorldManager hydros7WorldManager = Hydros7WorldManager.Instance;

                if (hydros7WorldManager.IsGamePlaying())
                    hydros7WorldManager.ToggleGamePause();
            });
        }
    }
}