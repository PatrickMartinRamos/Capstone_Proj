using UnityEngine;

namespace Stellarfarer
{
    public class DarkOverlayUI : TutorialOverlayUI
    {
        [SerializeField] private Material _reverseMaskInner;
        [SerializeField] private Material _reverseMaskOuter;

        public override void DisplayChooseHydriousTutorial()
        {
            ShowCanvas();
            RaycastCannotTarget();
            DisplayInner();
        }

        private void DisplayInner()
            => SetMaterial(_reverseMaskInner);

        private void DisplayOuter()
            => SetMaterial(_reverseMaskOuter);

        private void SetMaterial(Material material)
            => _visual.material = material;
    }
}