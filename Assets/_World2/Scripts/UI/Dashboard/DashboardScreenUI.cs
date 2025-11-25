using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Stellarfarer
{
    public class DashboardScreenUI : MonoBehaviour
    {
        [SerializeField] private Button _screen;
        [SerializeField] private Sprite _full;
        [SerializeField] private Sprite _visible;
        [SerializeField] private Color _unlit;

        private void Awake()
        {
            _screen.onClick.AddListener(() =>
            {
                if (!Hydros7WorldManager.Instance.IsGamePlaying() ||
                    DashboardManager.Instance.IsPoweredOff() ||
                    DashboardManager.Instance.IsSwitchedOn() ||
                    DashboardManager.Instance.IsExtracting())
                    return;

                DashboardManager.Instance.SwitchOn();
            });

            PowerOff();
        }

        private void Start()
            => _screen.image.ConfigureImageFromFullToVisibleSprite(
                    full: _full,
                    visible: _visible
                );

        private void SetColor(Color color)
            => _screen.image.DOColor(color, 1f);

        public void PowerOn(Color lit)
            => SetColor(lit);

        public void PowerOff()
            => SetColor(_unlit);

        public Vector3 GetPosition()
            => _screen.image.rectTransform.position;
    }
}