using System;
using UnityEngine;

namespace CapstoneProj.ControlPanelSystem
{
    public class CoordinateAnimation : MonoBehaviour
    {
        private const string SWIPEABLE = "Swipeable";
        private const string NOT_SWIPEABLE = "NotSwipeable";
        private const string SWIPE_START = "SwipeStart";
        private const string SWIPE_UP = "SwipeUp";
        private const string SWIPE_DOWN = "SwipeDown";
        private const string SWIPE_END = "SwipeEnd";

        [SerializeField] private Coordinate _coordinate;
        [SerializeField] private Animator _animator;

        private void Awake()
        {
            if (_animator == null)
                _animator = GetComponent<Animator>();
        }

        private void Start()
        {
            _coordinate.OnSwipeable
                += Coordinate_OnSwipeable;
            _coordinate.OnNotSwipeable
                += Coordinate_OnNotSwipeable;
            _coordinate.OnSwipeStart
                += Coordinate_OnSwipeStart;
            _coordinate.OnSwipeUp
                += Coordinate_OnSwipeUp;
            _coordinate.OnSwipeDown
                += Coordinate_OnSwipeDown;
            _coordinate.OnSwipeEnd
                += Coordinate_OnSwipeEnd;
        }

        private void OnDestroy()
        {
            if (_coordinate == null)
                return;

            _coordinate.OnSwipeable
                -= Coordinate_OnSwipeable;
            _coordinate.OnNotSwipeable
                -= Coordinate_OnNotSwipeable;
            _coordinate.OnSwipeStart
                -= Coordinate_OnSwipeStart;
            _coordinate.OnSwipeUp
                -= Coordinate_OnSwipeUp;
            _coordinate.OnSwipeDown
                -= Coordinate_OnSwipeDown;
            _coordinate.OnSwipeEnd
                -= Coordinate_OnSwipeEnd;
        }

        private void Coordinate_OnSwipeable(object sender, EventArgs e)
            => TriggerAnimation(SWIPEABLE);

        private void Coordinate_OnNotSwipeable(object sender, EventArgs e)
            => TriggerAnimation(NOT_SWIPEABLE);

        private void Coordinate_OnSwipeStart(object sender, EventArgs e)
            => TriggerAnimation(SWIPE_START);

        private void Coordinate_OnSwipeUp(object sender, EventArgs e)
            => TriggerAnimation(SWIPE_UP);

        private void Coordinate_OnSwipeDown(object sender, EventArgs e)
            => TriggerAnimation(SWIPE_DOWN);

        private void Coordinate_OnSwipeEnd(object sender, EventArgs e)
            => TriggerAnimation(SWIPE_END);

        private void TriggerAnimation(string trigger)
        {
            ResetAllTriggerAnimations();
            _animator.SetTrigger(trigger);
        }

        private void ResetTriggerAnimation(string trigger)
            => _animator.ResetTrigger(trigger);

        private void ResetAllTriggerAnimations()
        {
            ResetTriggerAnimation(SWIPEABLE);
            ResetTriggerAnimation(NOT_SWIPEABLE);
            ResetTriggerAnimation(SWIPE_START);
            ResetTriggerAnimation(SWIPE_UP);
            ResetTriggerAnimation(SWIPE_DOWN);
            ResetTriggerAnimation(SWIPE_END);
        }
    }
}