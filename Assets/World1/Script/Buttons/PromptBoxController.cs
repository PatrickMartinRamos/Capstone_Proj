using CapstoneProj.GameInputSystem;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;

public class PromptBoxController : MonoBehaviour
{
    [SerializeField] private float enableCloseTimer;
    private float timer;
    private void Update()
    {
        timer = Mathf.Clamp(timer-Time.deltaTime, 0.0f, enableCloseTimer);
        if (InputManager.Instance.IsTouching() && timer == 0)
            TriggerAction();
    }
    internal virtual void TriggerAction()
    {
        //gameObject.SetActive(false);
    }
    private void OnEnable()
    {
        timer = enableCloseTimer;
    }
}
 