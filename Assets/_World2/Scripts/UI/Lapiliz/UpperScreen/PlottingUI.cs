using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Stellarfarer
{
    public class PlottingUI : MonoBehaviour
    {
        [SerializeField] private Image _hydriousXRay;

        private GridManager _gridManager;

        private void Start()
        {
            if (GridManager.Instance != null)
                _gridManager = GridManager.Instance;
        }

        public void SetPlottingUI(HydriousUI hydriousUI)
        {
            _hydriousXRay.sprite = hydriousUI.GetXRaySprite();
            StartCoroutine(UpdateGrid());
        }

        public void ResetPlottingUI()
            => _gridManager.ResetGridManager();

        private IEnumerator UpdateGrid()
        {
            _gridManager.SetTarget();
            yield return new WaitForSeconds(1f);
            _gridManager.SetReachableTileUIList(_hydriousXRay);
            AzuliuzSpawnManager.Instance.Spawn();
        }

        public void Show()
            => SetVisiblity(true);

        public void Hide()
            => SetVisiblity(false);

        private void SetVisiblity(bool isVisible)
            => gameObject.SetActive(isVisible);
    }
}