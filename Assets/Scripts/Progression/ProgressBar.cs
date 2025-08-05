using System;
using CapstoneProj.MiscSystem;
using UnityEngine;

namespace CapstoneProj.ProgressSystem
{
    public class ProgressBar : SingletonBehaviour<ProgressBar>
    {
        public event EventHandler<OnStarProgressEventArgs> OnStarProgress;
        public class OnStarProgressEventArgs : EventArgs
        {
            public float ProgressPercent { get; private set; }

            public OnStarProgressEventArgs(float progressPercent)
            {
                ProgressPercent = progressPercent;
            }
        }

        private const float MAX_SIZE = 1f;
        private const float MIN_SIZE = 0f;
        private readonly Vector3 ORIGINAL_SCALE = new Vector3(MIN_SIZE, MAX_SIZE, MAX_SIZE);

        [SerializeField] private Transform _fillTransform;
        [SerializeField] private float _maxProgressTime;
        private float _elapsedTime;
        private Vector3 _scale;
        private Vector3 _previousScale;
        private float _progressPercent;
        private bool _isProgressing;
        private bool _isCompleted;
        private bool _isProgressActive;

        private void Start()
            => ResetProgressBar();

        public void ResetProgressBar()
        {
            _isProgressActive = true;
            _isCompleted = false;
            _isProgressing = false;
            _elapsedTime = 0f;
            _progressPercent = 0f;
            _scale = ORIGINAL_SCALE;
            _previousScale = ORIGINAL_SCALE;
            _fillTransform.localScale = ORIGINAL_SCALE;
        }

        public void Progress(float progressPercent)
        {
            _previousScale = _scale;
            _progressPercent = _scale.x;
            _progressPercent = progressPercent;
            _progressPercent = Mathf.Clamp(_progressPercent, MIN_SIZE, MAX_SIZE);
            _scale.x = _progressPercent;
            _isProgressing = true;
        }

        private void Update()
        {
            if (!_isProgressActive)
                return;

            if (_isCompleted)
            {
                // TODO: Event For Win
                _isProgressActive = false;
                return;
            }

            if (!_isProgressing)
                return;

            UpdateProgressBar();
        }

        private void UpdateProgressBar()
        {
            _elapsedTime += Time.deltaTime;
            float timer = Mathf.Clamp01(_elapsedTime / _maxProgressTime);

            _fillTransform.localScale = Vector3.Lerp(_previousScale, _scale, timer);

            const float MAX_TIME = 1;
            if (timer < MAX_TIME)
                return;

            OnStarProgress?.Invoke(this, new OnStarProgressEventArgs(_progressPercent));
            _isProgressing = false;
            _elapsedTime = 0f;
            _fillTransform.localScale = _scale;
            _isCompleted = _progressPercent == MAX_SIZE;
        }
    }
}