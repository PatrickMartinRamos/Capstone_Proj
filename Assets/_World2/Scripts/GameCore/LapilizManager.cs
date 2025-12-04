using System;
using UnityEngine;

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
        public event Func<Rect> OnCartesianPlaneToggleRectGot;
        public event Func<Rect> OnTab2RectGot;
        public event Func<Rect> OnLowerScreenRectGot;
        public event Func<Rect> OnTab1RectGot;
        public event Func<Rect> OnConfirmButtonRectGot;
        public event Func<Rect> OnCleanseButtonRectGot;
        public event Func<Rect> OnUpperScreenRectGot;

        private Activation _activation;

        public Rect GetCartesianPlaneToggleRect()
            => OnCartesianPlaneToggleRectGot?.Invoke() ?? Rect.zero;

        public Rect GetTab2Rect()
            => OnTab2RectGot?.Invoke() ?? Rect.zero;

        public Rect GetLowerScreenRect()
            => OnLowerScreenRectGot?.Invoke() ?? Rect.zero;

        public Rect GetUpperScreenRect()
            => OnUpperScreenRectGot?.Invoke() ?? Rect.zero;

        public Rect GetTab1Rect()
            => OnTab1RectGot?.Invoke() ?? Rect.zero;

        public Rect GetConfirmButtonRect()
            => OnConfirmButtonRectGot?.Invoke() ?? Rect.zero;

        public Rect GetCleansButtonRect()
            => OnCleanseButtonRectGot?.Invoke() ?? Rect.zero;

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