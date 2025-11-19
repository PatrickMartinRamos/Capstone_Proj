using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Stellarfarer
{
    public class ExtractionUI : MonoBehaviour
    {
        [SerializeField] private Image _visual;
        [SerializeField] private Sprite _full;
        [SerializeField] private Sprite _visible;

        private void Start()
        {
            _visual.ConfigureImageFromFullToVisibleSprite(
                full: _full,
                visible: _visible
            );
        }
    }
}