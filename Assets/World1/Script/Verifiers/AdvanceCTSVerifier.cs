using System.Collections.Generic;
using UnityEngine;

public class AdvanceCTSVerifier : VerifierManager
{
    [SerializeField] protected List<GameObject> secondVerifiers;
    protected List<int> secondAnswerKey;
    [SerializeField] int verifierSet, verifiedSet;

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
        }
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
    }
}
