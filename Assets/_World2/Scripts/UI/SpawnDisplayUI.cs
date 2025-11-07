using System;

namespace Stellarfarer
{
    public class SpawnDisplayUI : UnityEngine.MonoBehaviour
    {
        [UnityEngine.SerializeField] private UnityEngine.UI.Image _fill;

        private World2_SpawnManager _spawnManager;

        private void Start()
        {
            if (World2_SpawnManager.Instance != null)
            {
                _spawnManager = World2_SpawnManager.Instance;

                _spawnManager.OnNextSpawnChanged
                    += SpawnManager_OnNextSpawnChanged;
            }
        }

        private void OnDestroy()
        {
            if (_spawnManager != null)
            {
                _spawnManager.OnNextSpawnChanged
                    -= SpawnManager_OnNextSpawnChanged;
            }
        }

        private void SpawnManager_OnNextSpawnChanged()
            => _fill.color = _spawnManager.GetNextSpawnColor();
    }
}