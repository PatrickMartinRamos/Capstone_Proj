using UnityEngine;
namespace Stellarfarer
{
    public class PauseButtonUI : ResumeButtonUI
    {
        [SerializeField] private Sprite _full;
        [SerializeField] private Sprite _visible;

        private void Start()
            => _pause.image.ConfigureImageFromFullToVisibleSprite(
                    full: _full,
                    visible: _visible
                );

        protected override void ButtonAction(Hydros7WorldManager hydros7WorldManager)
            => hydros7WorldManager.PauseGame();
    }
}