namespace Stellarfarer
{
    [UnityEngine.RequireComponent(typeof(Hydrious))]
    public class HydriousInteraction : UnityEngine.MonoBehaviour
    {
        [UnityEngine.SerializeField] private Hydrious _hydrious;
        [UnityEngine.SerializeField] private UnityEngine.UI.Button _button;

        private void Awake()
        {
            _button.onClick.AddListener(() => _hydrious.Capture());
        }

        private void DisableInteractability()
            => SetInteractability(false);

        private void EnableInteractability()
            => SetInteractability(true);

        private void SetInteractability(bool isInteractable)
            => _button.interactable = isInteractable;
    }
}