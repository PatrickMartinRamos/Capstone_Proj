using CapstoneProj.EnumSystem;
using UnityEngine;

namespace CapstoneProj.ProgressSystem
{
    public class ProgressStar : MonoBehaviour
    {
        private const string POP_UP = "PopUp";

        [SerializeField] private ProgressStarType _progressStarType;
        [SerializeField] private Animator _animator;

        private void Awake()
        {
            if (_animator == null)
                _animator = GetComponent<Animator>();
        }

        private void Start()
            => ProgressBar.Instance.OnStarProgress +=
                ProgressBar_OnStarProgress;

        private void ProgressBar_OnStarProgress(object sender, ProgressBar.OnStarProgressEventArgs e)
        {
            float progressPercentThreshold = _progressStarType switch
            {
                ProgressStarType.Star1 => 1f / 3f,
                ProgressStarType.Star2 => 2f / 3f,
                ProgressStarType.Star3 => 3f / 3f,
                _ => 0f
            };

            if (e.ProgressPercent < progressPercentThreshold)
                return;

            _animator.SetTrigger(POP_UP);
        }
    }
}