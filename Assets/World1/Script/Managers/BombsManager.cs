using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BombsManager : MonoBehaviour
{
    // Serialized easier for Debugging
    [SerializeField] private List<GameObject> launchedBombs = new();
    [SerializeField] private int neutralizedBombs;
    private GameObject target;
    public GameObject targetBomb => target;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StageManager.Instance.bombsManager = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (neutralizedBombs == launchedBombs.Count && !StageManager.Instance.correctAnswerPanel.activeInHierarchy)
        {
            StageManager.Instance.correctAnswerPanel.SetActive(true);
            int score = 0;
            if (StageManager.Instance.currentTime >= StageManager.Instance.timeLimit / 2) score = 3;
            else score = StageManager.Instance.currentTime >= StageManager.Instance.timeLimit*(1/3)? 2 : 1;
                StageManager.Instance.correctAnswerPanel.GetComponentInChildren<Slider>().value = score;

            DataManager.Instance.SaveData(StageManager.Instance.StageNumber, score, StageManager.Instance.timeLimit - StageManager.Instance.currentTime);
        }
    }
    public bool RegisterBomb(GameObject bomb)
    {
        if (bomb == null) return false;
        Debug.Log(bomb);
        launchedBombs.Add(bomb);
        return true;
    }
    public void setTargetBomb(GameObject bomb)
    {
        target = bomb;
        StageManager.Instance.cannon.GetComponent<CannonMechanics>().SwitchShell(bomb.GetComponent<BombMechanics>().problemType);
        StageManager.Instance.problemType = this.targetBomb.GetComponent<BombMechanics>().problemType;
    }
    public void AddNeutralized()
    {
        neutralizedBombs++;
    }
}
