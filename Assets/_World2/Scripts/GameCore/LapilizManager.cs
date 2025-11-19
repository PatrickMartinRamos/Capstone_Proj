using System;

namespace Stellarfarer
{
    public class LapilizManager : SingletonBehaviour<LapilizManager>
    {
        private enum Activation
        {
            Activate,
            Deactivate
        }

        public event Action OnActivationChanged;
        public event Action OnExtractStarted;

        private Activation _activation;

        public void Activate()
            => SetActivation(Activation.Activate);
        public bool IsActivated()
            => IsActivation(Activation.Activate);

        public void Deactivate()
            => SetActivation(Activation.Deactivate);
        public bool IsDeactivated()
            => IsActivation(Activation.Deactivate);

        private void SetActivation(Activation activation)
        {
            _activation = activation;
            OnActivationChanged?.Invoke();
        }
        private bool IsActivation(Activation activation)
            => _activation == activation;

        public void Confirm()
        {
            HydriousUI hydriousUI = CaptureManager.Instance.GetHydriousUITarget();

            switch (hydriousUI.GetCleansingType())
            {
                case CleansingType.Plotting:
                    if (AzuliuzSpawnManager.Instance.TryExtract())
                        OnExtractStarted?.Invoke();
                    else
                    {
                        if (GridManager.Instance.TryMark())
                        {
                        }
                    }

                    break;
                case CleansingType.Midpoint:
                    if (NumberSlotManager.Instance.TryExtractMidpoint())
                        OnExtractStarted?.Invoke();
                    else
                    {

                    }

                    break;
                case CleansingType.Distance:
                    if (NumberSlotManager.Instance.TryExtractDistance())
                        OnExtractStarted?.Invoke();
                    else
                    {

                    }
                    break;
            }
        }
    }
}