using UnityEngine;
using UnityEngine.UI;

namespace Stellarfarer
{
    [RequireComponent(typeof(Button))]
    public class ResumeButtonUI : MonoBehaviour
    {
        [SerializeField] protected Button _pause;

        private void Awake()
        {
            if (_pause == null)
                _pause = GetComponent<Button>();

            _pause.onClick.AddListener(() =>
            {
                Hydros7WorldManager hydros7WorldManager = Hydros7WorldManager.Instance;

                if (hydros7WorldManager.IsGamePlaying() || hydros7WorldManager.IsDisplayingTutorial())
                    hydros7WorldManager.ToggleGamePause();
            });
        }
    }
}