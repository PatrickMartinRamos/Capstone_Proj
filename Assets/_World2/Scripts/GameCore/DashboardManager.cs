using System;
using UnityEngine;

namespace Stellarfarer
{
    public class DashboardManager : SingletonBehaviour<DashboardManager>
    {
        private enum PowerState
        {
            On,
            Off,
        }

        private enum SwitchState
        {
            On,
            Off
        }

        private enum ActionState
        {
            Idle,
            Extract,
        }

        public event Action OnPowerStateChanged;
        public event Action OnSwitchStateChanged;
        public event Func<Rect> OnDashboardRectGot;

        private PowerState _powerState = PowerState.Off;
        private SwitchState _switchState = SwitchState.Off;
        private ActionState _actionState = ActionState.Idle;

        private void Start()
            => ResetDashboard();

        private void ResetDashboard()
        {
            PowerOff();
            SwitchOff();
            Idle();
        }

        #region Power State Management
        public void PowerOn()
            => SetPowerState(PowerState.On);
        public bool IsPoweredOn()
            => IsPowerState(PowerState.On);

        public void PowerOff()
            => SetPowerState(PowerState.Off);
        public bool IsPoweredOff()
            => IsPowerState(PowerState.Off);

        private void SetPowerState(PowerState powerState)
        {
            if (IsPowerState(powerState))
                return;

            _powerState = powerState;
            OnPowerStateChanged?.Invoke();
        }
        private bool IsPowerState(PowerState powerState)
            => _powerState == powerState;
        #endregion

        #region Switch State Management
        public void SwitchOn()
            => SetSwitchState(SwitchState.On);
        public bool IsSwitchedOn()
            => IsSwitchState(SwitchState.On);

        public void SwitchOff()
            => SetSwitchState(SwitchState.Off);
        public bool IsSwitchedOff()
            => IsSwitchState(SwitchState.Off);

        private void SetSwitchState(SwitchState switchState)
        {
            if (IsSwitchState(switchState))
                return;

            _switchState = switchState;
            OnSwitchStateChanged?.Invoke();
        }
        private bool IsSwitchState(SwitchState switchState)
            => _switchState == switchState;
        #endregion

        #region Action State Management
        public void Idle()
            => SetActionState(ActionState.Idle);
        public bool IsIdling()
            => IsActionState(ActionState.Idle);

        public void Extract()
            => SetActionState(ActionState.Extract);
        public bool IsExtracting()
            => IsActionState(ActionState.Extract);

        private void SetActionState(ActionState actionState)
        {
            if (IsActionState(actionState))
                return;

            _actionState = actionState;
        }
        private bool IsActionState(ActionState actionState)
            => _actionState == actionState;
        #endregion

        public Rect GetDashboardRect()
            => OnDashboardRectGot?.Invoke() ?? Rect.zero;
    }
}