using UnityEngine;

namespace Stellarfarer
{
    public class SingletonBehaviour<T> : MonoBehaviour where T : MonoBehaviour
    {
        public static T Instance { private set; get; }

        protected virtual void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Debug.LogError($"{nameof(T)} already exits in {Instance.gameObject}. Duplicate instance found in {gameObject}");
                return;
            }

            Instance = this as T;
        }
    }
}