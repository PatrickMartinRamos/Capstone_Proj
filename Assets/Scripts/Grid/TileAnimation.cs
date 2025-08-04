using System;
using UnityEngine;

namespace CapstoneProj.GridSystem
{
    public class TileAnimation : MonoBehaviour
    {
        private const string SUCCESSFUL_INTERACTION = "SuccessfulInteraction"; // Animation trigger for successful interaction.
        private const string UNSUCCESSFUL_INTERACTION = "UnsuccessfulInteraction"; // Animation trigger for unsuccessful interaction.

        [SerializeField] private TileInteraction _tileInteraction; // Reference to the TileInteraction component.
        [SerializeField] private Animator _animator;

        private void Awake()
        {
            if (_tileInteraction == null)
                _tileInteraction = GetComponent<TileInteraction>(); // Get the TileInteraction component if not assigned.

            if (_animator == null)
                _animator = GetComponent<Animator>(); // Get the animator component if not assigned.
        }

        private void Start()
        {
            _tileInteraction.OnSuccessfulInteraction
                += TileInteraction_OnSuccessfulInteraction; // Subscribe to successful interaction event.
            _tileInteraction.OnUnsuccessfulInteraction
                += TileInteraction_OnUnsuccessfulInteraction; // Subscribe to unsuccessful interaction event.
        }

        private void OnDestroy()
        {
            if (_tileInteraction != null)
            {
                _tileInteraction.OnSuccessfulInteraction
                    -= TileInteraction_OnSuccessfulInteraction; // Unsubscribe from successful interaction event.
                _tileInteraction.OnUnsuccessfulInteraction
                    -= TileInteraction_OnUnsuccessfulInteraction; // Unsubscribe from unsuccessful interaction event.
            }
        }

        private void TileInteraction_OnUnsuccessfulInteraction(object sender, EventArgs e)
            => _animator.SetTrigger(UNSUCCESSFUL_INTERACTION); // Trigger the unsuccessful interaction animation.

        private void TileInteraction_OnSuccessfulInteraction(object sender, EventArgs e)
            => _animator.SetTrigger(SUCCESSFUL_INTERACTION); // Trigger the successful interaction animation.
    }
}