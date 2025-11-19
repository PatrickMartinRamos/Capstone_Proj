using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject correctAnsPanel, wrongAnsPanel, promptBox, confirmationBox;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StageManager.Instance.uiManager = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
