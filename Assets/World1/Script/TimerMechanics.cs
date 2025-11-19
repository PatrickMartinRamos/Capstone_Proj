using UnityEngine;
using UnityEngine.UI;

public class TimerMechanics : MonoBehaviour
{
    [SerializeField] float currentTime = 100;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        StageManager.Instance.timer = this.gameObject.GetComponent<Slider>();
        StageManager.Instance.timer.maxValue = StageManager.Instance.timeLimit;
    }
    private void Start()
    {
        currentTime = StageManager.Instance.currentTime;
    }
    public float GetCurrentTime()
    {
        return currentTime;
    }
    public void DeductTime(float deduction)
    {
        currentTime = currentTime - deduction;
    }

    // Update is called once per frame
    void Update()
    {
        currentTime = Mathf.Clamp(currentTime-Time.deltaTime, 0, StageManager.Instance.timeLimit);
        StageManager.Instance.timer.value = currentTime;
        if(currentTime == 0)
        {
            StageManager.Instance.WrongAnswerPanel.SetActive(true);
        }
    }
}
