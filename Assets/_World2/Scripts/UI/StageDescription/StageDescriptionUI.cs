using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Stellarfarer
{
    [RequireComponent(typeof(Button))]
    public class StageDescriptionUI : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private TextMeshProUGUI _stageLevelDescriptionUI;
        [SerializeField] private ObjectiveUi _plottingObjectiveUI;
        [SerializeField] private ObjectiveUi _midpointObjectiveUI;
        [SerializeField] private ObjectiveUi _distanceObjectiveUI;

        private Hydros7WorldManager _hydros7WorldManager;

        private void Awake()
        {
            if (_button == null)
                _button = GetComponent<Button>();

            _button.onClick.AddListener(() =>
            {
                if (Hydros7WorldManager.Instance.IsDisplayingStageDescription())
                {
                    Hide();
                    Hydros7WorldManager.Instance.Wait();
                }
            });
        }

        private void Start()
        {
            if (Hydros7WorldManager.Instance != null)
            {
                _hydros7WorldManager = Hydros7WorldManager.Instance;

                _hydros7WorldManager.OnGameStateChanged
                    += Hydros7WorldManager_OnGameStateChanged;
            }

            Hide();
        }

        private void OnDestroy()
        {
            if (_hydros7WorldManager != null)
            {
                _hydros7WorldManager.OnGameStateChanged
                    -= Hydros7WorldManager_OnGameStateChanged;
            }
        }

        private void Hydros7WorldManager_OnGameStateChanged()
        {
            if (_hydros7WorldManager.IsDisplayingStageDescription())
            {
                Show();

                int stageLevel = _hydros7WorldManager.GetStageID();
                _stageLevelDescriptionUI.text = $"Stage {stageLevel}";

                CleansingType cleansingType = _hydros7WorldManager.GetCleansingType();
                _distanceObjectiveUI.TryDisplay(cleansingType);
                _midpointObjectiveUI.TryDisplay(cleansingType);
                _plottingObjectiveUI.TryDisplay(cleansingType);
            }
        }

        private void Show()
            => SetVisibility(true);

        private void Hide()
            => SetVisibility(false);

        private void SetVisibility(bool isVisible)
            => gameObject.SetActive(isVisible);
    }
}