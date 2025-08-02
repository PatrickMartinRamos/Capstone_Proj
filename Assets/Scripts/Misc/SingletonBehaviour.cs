using UnityEngine;

namespace CapstoneProj.MiscSystem
{
    public class SingletonBehaviour<T> : MonoBehaviour where T : Component
    {
        public static T Instance { get; private set; }

        protected virtual void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Debug.LogWarning($"An instance of {typeof(T).Name} already exists in {gameObject.name}. Destroying the new instance.");
                Destroy(gameObject);
                return;
            }
            
            Instance = this as T;
        }
    }
}