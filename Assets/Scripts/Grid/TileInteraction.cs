using System;
using UnityEngine;

namespace CapstoneProj.GridSystem
{
    public class TileInteraction : MonoBehaviour
    {
        public event EventHandler OnSuccessfulInteraction; // Event to notify when a successful interaction occurs.
        public event EventHandler OnUnsuccessfulInteraction; // Event to notify when an unsuccessful interaction occurs.

        public void SuccessfulInteraction()
            => OnSuccessfulInteraction?.Invoke(this, EventArgs.Empty); // Invoke the successful interaction event.

        public void UnsuccessfulInteraction()
            => OnUnsuccessfulInteraction?.Invoke(this, EventArgs.Empty); // Invoke the unsuccessful interaction event.
    }
}