using System;
using UnityEngine;

namespace CapstoneProj.ControlPanelSystem
{
    public class DefuseButtonAnimation : MonoBehaviour
    {
        private const string INTERACTABLE = "Interactable";
        private const string NOT_INTERACTABLE = "NotInteractable";
        private const string CORRECT_CLICK = "CorrectClick";
        private const string INCORRECT_CLICK = "IncorrectClick";

        [SerializeField] private Animator _animator;

        private void Awake()
        {
            if (_animator == null)
                _animator = GetComponent<Animator>();
        }

        private void Start()
        {
            DefuseButton.Instance.OnInteractable
                += DefuseButton_OnInteractable;
            DefuseButton.Instance.OnNotInteractable
                += DefuseButton_OnNotInteractable;
            DefuseButton.Instance.OnCorrectClick
                += DefuseButton_OnCorrectClick;
            DefuseButton.Instance.OnIncorrectClick
                += DefuseButton_OnIncorrectClick;
        }

        private void OnDestroy()
        {
            if (CartesianPlaneToggle.Instance == null)
                return;

            DefuseButton.Instance.OnInteractable
                -= DefuseButton_OnInteractable;
            DefuseButton.Instance.OnNotInteractable
                -= DefuseButton_OnNotInteractable;
            DefuseButton.Instance.OnCorrectClick
                -= DefuseButton_OnCorrectClick;
            DefuseButton.Instance.OnIncorrectClick
                -= DefuseButton_OnIncorrectClick;
        }

        private void DefuseButton_OnInteractable(object sender, EventArgs e)
            => TriggerAnimation(INTERACTABLE);

        private void DefuseButton_OnNotInteractable(object sender, EventArgs e)
            => TriggerAnimation(NOT_INTERACTABLE);

        private void DefuseButton_OnCorrectClick(object sender, EventArgs e)
            => TriggerAnimation(CORRECT_CLICK);

        private void DefuseButton_OnIncorrectClick(object sender, EventArgs e)
            => TriggerAnimation(INCORRECT_CLICK);

        private void TriggerAnimation(string trigger)
        {
            ResetAllTriggerAnimations();
            _animator.SetTrigger(trigger);
        }

        private void ResetTriggerAnimation(string trigger)
            => _animator.ResetTrigger(trigger);

        private void ResetAllTriggerAnimations()
        {
            ResetTriggerAnimation(INTERACTABLE);
            ResetTriggerAnimation(NOT_INTERACTABLE);
            ResetTriggerAnimation(CORRECT_CLICK);
            ResetTriggerAnimation(INCORRECT_CLICK);
        }
    }
}