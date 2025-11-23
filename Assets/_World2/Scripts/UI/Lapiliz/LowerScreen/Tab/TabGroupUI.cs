using System;
using UnityEngine;

namespace Stellarfarer
{
    public class TabGroupUI : MonoBehaviour
    {
        public event Action<bool, bool> OnTabSelectionChanged;

        [SerializeField] private TabUI _tab1;
        [SerializeField] private TabUI _tab2;

        private void Awake()
        {
            _tab1.OnIsOnChanged
                += HandleTabChange;
            _tab2.OnIsOnChanged
                += HandleTabChange;
        }

        private void Start()
            => _tab1.On();

        private void OnDestroy()
        {
            _tab1.OnIsOnChanged
                -= HandleTabChange;
            _tab2.OnIsOnChanged
                -= HandleTabChange;
        }

        private void HandleTabChange()
            => OnTabSelectionChanged?.Invoke(_tab1.IsOn(), _tab2.IsOn());
    }
}