using UnityEngine;

namespace Stellarfarer
{
    public class HydriousSpawnDisplayUI : MonoBehaviour
    {
        [SerializeField] private UnityEngine.UI.Image _fill;

        private HydriousSpawnManager _hydriousSpawnManager;

        private void Start()
        {
            if (HydriousSpawnManager.Instance != null)
            {
                _hydriousSpawnManager = HydriousSpawnManager.Instance;

                _hydriousSpawnManager.OnNextSpawnChanged
                    += HydriousSpawnManager_OnNextSpawnChanged;
            }
        }

        private void OnDestroy()
        {
            if (_hydriousSpawnManager != null)
            {
                _hydriousSpawnManager.OnNextSpawnChanged
                    -= HydriousSpawnManager_OnNextSpawnChanged;
            }
        }

        private void HydriousSpawnManager_OnNextSpawnChanged()
            => _fill.color = _hydriousSpawnManager.GetNextSpawnColor();
    }
}