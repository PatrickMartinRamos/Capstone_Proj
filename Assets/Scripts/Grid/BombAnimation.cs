using CapstoneProj.EnumSystem;
using UnityEditor.Animations;
using UnityEngine;

namespace CapstoneProj.GridSystem
{
    public class BombAnimation : MonoBehaviour
    {
        [SerializeField] private BombAnimationType _bombAnimationType;
        [SerializeField] private Animator _animator;
        [SerializeField] private AnimatorController _bombAnimatorController;
        [SerializeField] private AnimatorController _bombMarkerAnimatorController;
        
        public void SetBombAnimationType(BombAnimationType bombAnimationType)
        {
            _bombAnimationType = bombAnimationType;
            UpdateAnimatorController();
        }

        private void UpdateAnimatorController()
        {
            _animator.runtimeAnimatorController = _bombAnimationType switch
            {
                BombAnimationType.Bomb => _bombAnimatorController,
                BombAnimationType.BombMarker => _bombMarkerAnimatorController,
                _ => null
            };
        }

        private void Awake()
        {
            if (_animator == null)
                _animator = GetComponent<Animator>();

            if (_bombAnimatorController == null)
            {
                Debug.LogError("BombAnimator is not assigned in the inspector.");
                return;
            }

            if (_bombMarkerAnimatorController == null)
            {
                Debug.LogError("BombMarkerAnimator is not assigned in the inspector.");
                return;
            }
        }
    }
}