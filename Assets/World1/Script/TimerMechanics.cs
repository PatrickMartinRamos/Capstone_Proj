using UnityEngine;
using UnityEngine.UI;

public class TimerMechanics : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        StageManager.Instance.timer = this.gameObject.GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
