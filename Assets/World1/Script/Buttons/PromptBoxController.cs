using CapstoneProj.GameInputSystem;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PromptBoxController : MonoBehaviour
{
    [SerializeField] private float enableCloseTimer;
    [SerializeField] private List<GameObject> ObjectsToClose = new();
    private float timer;
    private void Update()
    {
        timer = Mathf.Clamp(timer-Time.deltaTime, 0.0f, enableCloseTimer);
        if (InputManager.Instance.IsTouching() && timer == 0)
            TriggerAction();
    }
    internal virtual void TriggerAction()
    {
        gameObject.SetActive(false);
        if (ObjectsToClose.Count != 0)
        {
            foreach (GameObject obj in ObjectsToClose)
            {
                obj.SetActive(false);
            }
        }
    }
    private void OnEnable()
    {
        timer = enableCloseTimer;
    }
}
 