using UnityEngine;
using UnityEngine.UI;

namespace Stellarfarer
{
    public class CartesianPlaneUI : MonoBehaviour
    {

        private GridManager _gridManager;

        private void Start()
        {
            if (GridManager.Instance != null)
            {
                _gridManager = GridManager.Instance;

                _gridManager.OnCartesianPlaneToggled
                    += GridManager_OnCartesianPlaneToggled;
            }

            Hide();
        }

        private void OnDestroy()
        {
            if (_gridManager != null)
            {
                _gridManager.OnCartesianPlaneToggled
                    -= GridManager_OnCartesianPlaneToggled;
            }
        }

        private void GridManager_OnCartesianPlaneToggled(bool isToggled)
        {
            if (isToggled)
                Show();
            else
                Hide();
        }

        private void Show()
            => SetVisiblity(true);

        private void Hide()
            => SetVisiblity(false);

        private void SetVisiblity(bool isVisible)
            => gameObject.SetActive(isVisible);
    }
}