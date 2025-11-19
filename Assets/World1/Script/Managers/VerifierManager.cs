using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class VerifierManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> Verifiers;
    private List<int> answerKey;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void VerifyAnswers()
    {
        answerKey = StageManager.Instance.problem.Answers();
        int numCorrect = 0;
        for (int i = 0;  i < Verifiers.Count; i++)
        {
            if (Verifiers[i].GetComponent<VerifierMechanics>().VerifyShape(answerKey[i]))
                numCorrect++;
        }
        if (numCorrect == Verifiers.Count)
        {
            StageManager.Instance.StartSuccessSequence();
        }
        else
        {
            Difficulty diff = StageManager.Instance.problem.stageDifficulty;
            int timeDeduction = 0;
            switch (diff)
            {
                case Difficulty.easy:
                    timeDeduction = 1;
                    break;
                case Difficulty.normal:
                    timeDeduction = 2; break;
                case Difficulty.hard:
                    timeDeduction = 3; break;
            }
            StageManager.Instance.NotificationText.text = $"Combination Error... (Time - {timeDeduction})";
            StageManager.Instance.DeductTime(timeDeduction);
        }

    }
}
