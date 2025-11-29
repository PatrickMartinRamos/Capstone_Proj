using System.Collections.Generic;
using UnityEngine;

public class AdvanceCTSVerifier : VerifierManager
{
    [SerializeField] protected List<GameObject> secondVerifiers;
    protected List<int> secondAnswerKey;
    [SerializeField] int verifierSet = 2, verifiedSet;
    [SerializeField] SpriteRenderer answerIndicator1, answerIndicator2;
    [SerializeField] List<GameObject> set1Gameobjects, set2Gameobjects, verifiedGears;
    [SerializeField] GameObject LeftSide, RightSide;

    private void Start()
    {
        answerIndicator1.color = Color.yellow;
        answerIndicator2.color = Color.grey;
    }
    public override void VerifyAnswers()
    {
        if (verifiedSet == verifierSet)
        {
            StageManager.Instance.StartSuccessSequence();
        }
    }
    public void VerifySetOne()
    {
        answerKey = StageManager.Instance.problem.gameObject.GetComponent<AdvanceCTSProblemLoader>().Answers(1);
        int numCorrect = 0;
        for (int i = 0; i < Verifiers.Count; i++)
        {
            if (Verifiers[i].GetComponent<VerifierMechanics>().VerifyShape(answerKey[i]))
                numCorrect++;
            verifiedGears.Add(Verifiers[i].GetComponent<VerifierMechanics>().embedShape);
        }
        if (numCorrect == Verifiers.Count)
        {
            verifiedSet += 1;
            answerIndicator1.color = Color.green;
            OpenSetTwo();
            CloseSetOne();
            CleanCraftBox();
            ReformatGiven();

        }
        else answerIndicator1.color = Color.red;

    }
    public void VerifySetTwo()
    {
        secondAnswerKey = StageManager.Instance.problem.gameObject.GetComponent<AdvanceCTSProblemLoader>().Answers(2);
        int numCorrect = 0;
        for (int i = 0; i < Verifiers.Count; i++)
        {
            if (secondVerifiers[i].GetComponent<VerifierMechanics>().VerifyShape(answerKey[i]))
                numCorrect++;
        }
        if (numCorrect == secondVerifiers.Count)
        {
            verifiedSet += 1;
            answerIndicator2.color = Color.green;
            
        }
        else answerIndicator2.color = Color.red;

    }
    void OpenSetTwo()
    {
        foreach(var gO in set2Gameobjects)
        {
            gO.gameObject.SetActive(true);
        }
    }
    void CloseSetOne()
    {
        foreach (var gO in set1Gameobjects)
        {
            gO.gameObject.SetActive(false);
        }
    }
    void CleanCraftBox()
    {
        for (int i = 0; i < StageManager.Instance.craftArea.transform.childCount; i++)
        {
            StageManager.Instance.craftArea.transform.GetChild(i).gameObject.SetActive(false);
        }
        for (int i = 0; i < StageManager.Instance.craftArea2.transform.childCount; i++)
        {
            StageManager.Instance.craftArea2.transform.GetChild(i).gameObject.SetActive(false);
        }
    }
    void ReformatGiven()
    {
        for(int i = 1; i<LeftSide.transform.childCount; i++)
        {
            LeftSide.transform.GetChild(i).gameObject.SetActive(false);

        }
        for (int i = 1; i < RightSide.transform.childCount; i++)
        {
            RightSide.transform.GetChild(i).gameObject.SetActive(false);

        }


        verifiedGears[0].transform.SetParent(LeftSide.transform);
        verifiedGears[0].GetComponent<Shapes>().FixScale();
        verifiedGears[0].transform.localPosition = new Vector3(-1.1f, 0, 0);

        verifiedGears[1].transform.SetParent(LeftSide.transform);
        verifiedGears[1].GetComponent<Shapes>().FixScale();
        verifiedGears[1].transform.localPosition = new Vector3(1.1f, 0, 0);

        verifiedGears[2].transform.SetParent(RightSide.transform);
        verifiedGears[2].GetComponent<Shapes>().FixScale();
        verifiedGears[2].transform.localPosition = new Vector3(0, 0, 0);
    }
}
