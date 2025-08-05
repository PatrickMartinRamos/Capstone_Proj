using System;
using UnityEngine;

namespace CapstoneProj.GridSystem
{
    public class GridNavigatorAnimation : MonoBehaviour
    {
        private const string SCREEN_TRAVERSAL = "ScreenTraversal";
        private const float ANIMATION_CLIP_DURATION = 1f;

        [SerializeField] private Animator _animator;
        [SerializeField] private Animation _animation ;

        private void Awake()
        {
            if (_animator == null)
                _animator = GetComponent<Animator>();
        }

        private void Start()
        {
            GridNavigator.Instance.OnDestroySelf
                += GridNavigator_OnDestroySelf;
        }

        private void OnDestroy()
        {
            if (GridNavigator.Instance == null)
                return;

            GridNavigator.Instance.OnDestroySelf
                -= GridNavigator_OnDestroySelf;
        }

        private void GridNavigator_OnDestroySelf(object sender, EventArgs e)
        {
            Tile originalTopTile = BottomScreenBombSpawner.Instance.GetOriginalTopTile();

            Vector3 startPos = transform.position;
            Vector3 endPos = originalTopTile.GetTileTransform().position;

            // X, Y, Z position curves
            AnimationCurve curveX = AnimationCurve.Linear(0f, startPos.x, ANIMATION_CLIP_DURATION, endPos.x);
            AnimationCurve curveY = AnimationCurve.Linear(0f, startPos.y, ANIMATION_CLIP_DURATION, endPos.y);
            AnimationCurve curveScale = new AnimationCurve(
                new Keyframe(0f, 0.5f),   // Start at scale 1
                new Keyframe(0.5f, 5f),  // Scale up to 5 over 30 seconds
                new Keyframe(1f, 1f)   // Scale back down to 1 by 60 seconds
            );

            AnimatorOverrideController overrideController = new AnimatorOverrideController(_animator.runtimeAnimatorController);
            AnimationClip overrideClip = new AnimationClip();
            overrideClip.legacy = false;
            overrideClip.name = "GridNavigatorScreenTraversal";

            // Set dynamic curves
            overrideClip.SetCurve("", typeof(Transform), "localPosition.x", curveX);
            overrideClip.SetCurve("", typeof(Transform), "localPosition.y", curveY);
            overrideClip.SetCurve("", typeof(Transform), "localScale.x", curveScale);
            overrideClip.SetCurve("", typeof(Transform), "localScale.y", curveScale);

            AnimationEvent animationEvent = new AnimationEvent()
            {
                functionName = "DestroySelfContinuation",
                time = 1f
            };
            overrideClip.AddEvent(animationEvent);

            overrideController["GridNavigatorScreenTraversal"] = overrideClip;
            _animator.runtimeAnimatorController = overrideController;

            TriggerAnimation(SCREEN_TRAVERSAL);
        }

        private void TriggerAnimation(string trigger)
            => _animator.SetTrigger(trigger);
    }
}