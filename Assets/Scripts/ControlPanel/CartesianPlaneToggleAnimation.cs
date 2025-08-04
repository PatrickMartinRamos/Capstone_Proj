using System;
using UnityEngine;

namespace CapstoneProj.ControlPanelSystem
{
    public class CartesianPlaneToggleAnimation : MonoBehaviour
    {
        private const string SELECT = "Select";
        private const string DESELECT = "Deselect";
        private const string TOGGLEABLE = "Toggleable";
        private const string UNTOGGLEABLE = "Untoggleable";

        [SerializeField] private Animator _animator;

        private void Awake()
        {
            if (_animator == null)
                _animator = GetComponent<Animator>();
        }

        private void Start()
        {
            CartesianPlaneToggle.Instance.OnToggleable
                += CartesianPlaneButton_OnToggleable;
            CartesianPlaneToggle.Instance.OnUntoggleable
                += CartesianPlaneButton_OnUntoggleable;
            CartesianPlaneToggle.Instance.OnSelected
                += CartesianPlaneButton_OnSelected;
            CartesianPlaneToggle.Instance.OnDeselected
                += CartesianPlaneButton_OnDeselected;
        }

        private void OnDestroy()
        {
            if (CartesianPlaneToggle.Instance == null)
                return;

            CartesianPlaneToggle.Instance.OnToggleable
                -= CartesianPlaneButton_OnToggleable;
            CartesianPlaneToggle.Instance.OnUntoggleable
                -= CartesianPlaneButton_OnUntoggleable;
            CartesianPlaneToggle.Instance.OnSelected
                -= CartesianPlaneButton_OnSelected;
            CartesianPlaneToggle.Instance.OnDeselected
                -= CartesianPlaneButton_OnDeselected;
        }

        private void CartesianPlaneButton_OnUntoggleable(object sender, EventArgs e)
            => TriggerAnimation(UNTOGGLEABLE);

        private void CartesianPlaneButton_OnToggleable(object sender, EventArgs e)
            => TriggerAnimation(TOGGLEABLE);

        private void CartesianPlaneButton_OnDeselected(object sender, EventArgs e)
            => TriggerAnimation(DESELECT);

        private void CartesianPlaneButton_OnSelected(object sender, EventArgs e)
            => TriggerAnimation(SELECT);

        private void TriggerAnimation(string trigger)
            => _animator.SetTrigger(trigger);
    }
}