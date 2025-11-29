using UnityEngine;
using UnityEngine.UI;

namespace Stellarfarer
{
    [RequireComponent(typeof(Button))]
    public class DarkOverlayUI : TutorialOverlayUI
    {
        [SerializeField] private Button _button;
        [SerializeField] private Material _reverseMaskInner;
        [SerializeField] private Material _reverseMaskOuter;


        protected override void Awake()
        {
            if (_button)
                _button = GetComponent<Button>();

            _button.onClick.AddListener(() =>
            {
                if (Hydros7WorldManager.Instance.IsDisplayingTutorial() &&
                TutorialManager.Instance.IsDisplayingProgressionTutorial())
                {
                    Hydros7WorldManager.Instance.TryDisplayTutorial(TutorialManager.Instance.Wait);
                    Hydros7WorldManager.Instance.Countdown();
                    int stageID = Hydros7WorldManager.Instance.GetStageID();
                    PlayerPrefs.SetInt($"Stage {stageID}", 1);
                }
            });

            base.Awake();
        }

        private void DisplayFullScreentargetable()
        {
            Show();
            RaycastCanTarget();
            FullAnchors();
            ResetOffset();
        }

        private void DisplayFullScreenUntargetable()
        {
            Show();
            RaycastCannotTarget();
            FullAnchors();
            ResetOffset();
        }

        private void DisplayFullScreenUntargetableInner()
        {
            DisplayFullScreenUntargetable();
            DisplayInner();
        }

        private void DisplayFullScreenTargetableOuter()
        {
            DisplayFullScreentargetable();
            DisplayOuter();
        }

        private void DisplayFullScreenUntargetableOuter()
        {
            DisplayFullScreenUntargetable();
            DisplayOuter();
        }

        public override void DisplayChooseHydriousTutorial()
            => DisplayFullScreenUntargetableInner();

        public override void DisplayClickDashboardTutorial()
            => DisplayFullScreenUntargetableOuter();

        public override void DisplayPlottingCartesianPlaneTutorial()
            => DisplayFullScreenUntargetableOuter();

        public override void DisplayBottom(float offset)
        {
            DisplayFullScreenUntargetableOuter();
            OffsetTop(offset);
        }

        public override void DisplayTop(float offset)
        {
            DisplayFullScreenUntargetableInner();
            OffsetBottom(offset);
        }

        public override void DisplaySwitchToTab2Tutorial()
            => DisplayFullScreenUntargetableOuter();

        public override void DisplaySwitchToTab1Tutorial()
            => DisplayFullScreenUntargetableOuter();

        public override void DisplayCleanseTutorial()
            => DisplayFullScreenUntargetableOuter();

        public override void DisplayProgressionTutorial()
            => DisplayFullScreenTargetableOuter();

        public void DisplayMidpointTutorial()
            => DisplayFullScreenUntargetableOuter();

        private void DisplayInner()
            => SetMaterial(_reverseMaskInner);

        private void DisplayOuter()
            => SetMaterial(_reverseMaskOuter);

        private void SetMaterial(Material material)
            => _visual.material = material;
    }
}