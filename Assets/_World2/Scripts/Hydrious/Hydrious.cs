using System;
using UnityEngine;

namespace Stellarfarer
{
    [RequireComponent(typeof(RectTransform))]
    [RequireComponent(typeof(HydriousMovement))]
    [RequireComponent(typeof(HydriousInteraction))]
    public class Hydrious : MonoBehaviour
    {
        private enum State
        {
            Idle,
            Move,
            Capture,
        }

        public event Action OnIdle;
        public event Action OnMove;
        public event Action OnCapture;

        [SerializeField] private HydriousSO _hydriousSO;
        [SerializeField] private HydriousMovement _hydriousMovement;
        [SerializeField] private HydriousInteraction _hydriousInteraction;

        private State _state = State.Idle;

        private void Awake()
        {
            if (_hydriousMovement == null)
                _hydriousMovement = GetComponent<HydriousMovement>();

            if (_hydriousInteraction == null)
                _hydriousInteraction = GetComponent<HydriousInteraction>();
        }

        private void Start()
            => Idle();

        public void Idle()
        {
            UpdateState(State.Idle);
            OnIdle?.Invoke();
        }

        public void Move()
        {
            UpdateState(State.Move);
            OnMove?.Invoke();
        }

        public void Capture()
        {
            UpdateState(State.Capture);
            OnCapture?.Invoke();
            CaptureManager.Instance.Capture(this);
        }

        private void UpdateState(State state)
            => _state = state;

        public HydriousSO GetHydriousSO()
            => _hydriousSO;

        public void DestroySelf()
            => Destroy(gameObject);

        public RectTransform GetRectTransform()
            => _hydriousMovement.GetRectTransform();

        public void SetParent(Transform parent)
            => _hydriousMovement.SetParent(parent);

        public static Hydrious SpawnHydrious(
            HydriousSO hydriousSO,
            RectTransform parent,
            Vector2 spawnPosition)
        {
            Transform hydriousTransform = Instantiate(hydriousSO.Prefab, parent);

            if (hydriousTransform.TryGetComponent(out RectTransform rectTransform))
                rectTransform.anchoredPosition = spawnPosition;

            return hydriousTransform.GetComponent<Hydrious>();
        }
    }
}