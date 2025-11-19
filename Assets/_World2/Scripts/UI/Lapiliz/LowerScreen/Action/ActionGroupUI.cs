using System;
using UnityEngine;

namespace Stellarfarer
{
    public class ActionGroupUI : MonoBehaviour
    {
        [SerializeField] private CartesianPlaneToggleUI _cartesianPlaneToggleUI;
        [SerializeField] private ConfirmButtonUI _confirmButtonUI;

        public void EnableCartesianPlaneToggle()
            => _cartesianPlaneToggleUI.EnableToggleInteractability();

        public void DisableCartesianPlaneToggle()
            => _cartesianPlaneToggleUI.DisableToggleInteractability();

        public void Show()
            => SetVisibility(true);

        public void Hide()
            => SetVisibility(false);

        private void SetVisibility(bool isVisible)
            => gameObject.SetActive(isVisible);
    }
}