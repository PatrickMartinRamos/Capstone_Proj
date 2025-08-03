using System;
using CapstoneProj.MiscSystem;
using UnityEngine;

namespace CapstoneProj.GridSystem
{
    public class BombSpawnTimer : SingletonBehaviour<BombSpawnTimer>
    {
        // TODO: MAKE SURE TILE SPAWNER IS READY BEFORE THIS

        public event EventHandler OnBombSpawnTimerRunOut;

        [SerializeField] private Transform _fillTransform;
        [SerializeField] private float _maxSpawnTime;
        [SerializeField] private float _maxReloadTime;
        private float _elapsedTime;
        private bool _isSpawnActive;
        private bool _isReloading;
        private Vector3 _maxSize = Vector3.one;
        private Vector3 _minSize = new Vector3(0f, 1f, 1f);

        private void Start()
            => ResetCountdown();

        private void ResetCountdown()
        {
            _elapsedTime = 0;
            _fillTransform.localScale = _maxSize;
            _isSpawnActive = true;
            _isReloading = false;
        }

        private void ResetReload()
        {
            _elapsedTime = 0;
            _fillTransform.localScale = _minSize;
            _isReloading = true;
        }

        private void Update()
        {
            if (!_isSpawnActive)
                return;

            if (_isReloading)
                UpdateMonsterSpawnTimer(_maxReloadTime, _minSize, _maxSize, ResetCountdown);
            else
                UpdateMonsterSpawnTimer(_maxSpawnTime, _maxSize, _minSize, ResetReload);
        }

        private void UpdateMonsterSpawnTimer(float maxTime, Vector3 startSize, Vector3 endSize, Action resetCallback)
        {
            _elapsedTime += Time.deltaTime;
            float timer = Mathf.Clamp01(_elapsedTime / maxTime);

            _fillTransform.localScale = Vector3.Lerp(startSize, endSize, timer);

            const float MAX_TIME = 1;
            if (timer < MAX_TIME)
                return;

            resetCallback?.Invoke();

            if (!_isReloading)
                return;
            
            OnBombSpawnTimerRunOut?.Invoke(this, EventArgs.Empty);
        }
    }
}