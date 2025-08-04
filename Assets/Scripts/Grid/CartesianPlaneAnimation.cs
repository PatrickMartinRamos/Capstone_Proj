using System;
using CapstoneProj.ControlPanelSystem;
using UnityEngine;

namespace CapstoneProj.GridSystem
{
    public class CartesianPlaneAnimation : MonoBehaviour
    {
        private const string SHOW = "Show";
        private const string HIDE = "Hide";

        [SerializeField] private Animator _animator;

        private void Awake()
        {
            if (_animator == null)
                _animator = GetComponent<Animator>();
        }

        private void Start()
        {
            CartesianPlaneToggle.Instance.OnSelected
                += CartesianPlaneButton_OnSelected;
            CartesianPlaneToggle.Instance.OnDeselected
                += CartesianPlaneButton_OnDeselected;
        }

        private void OnDestroy()
        {
            if (CartesianPlaneToggle.Instance == null)
                return;

            CartesianPlaneToggle.Instance.OnSelected
                -= CartesianPlaneButton_OnSelected;
            CartesianPlaneToggle.Instance.OnDeselected
                -= CartesianPlaneButton_OnDeselected;
        }

        private void CartesianPlaneButton_OnDeselected(object sender, EventArgs e)
            => TriggerAnimation(HIDE);

        private void CartesianPlaneButton_OnSelected(object sender, EventArgs e)
            => TriggerAnimation(SHOW);

        private void TriggerAnimation(string trigger)
            => _animator.SetTrigger(trigger);
    }
}