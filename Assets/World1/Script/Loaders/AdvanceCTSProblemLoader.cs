using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AdvanceCTSProblemLoader : CompletingSquareProblemLoader
{
    [SerializeField] protected List<int> answers1 = new List<int>();
    [SerializeField] protected List<int> answers2 = new List<int>();
    [SerializeField] private Transform rSide;
    private GameObject constant;

    protected override void Start()
    {
        base.Start();
        constant = StageManager.Instance.constant;
    }
    public virtual List<int> Answers(int set)
    {
        return set == 1 ? answers1 : answers2;
    }
    public override void LoadProblem()
    {
        GetLevelDifficulty();
        InstantiateGivenAdvance(leftSide.transform, rSide, ProblemMarkers[0], ProblemMarkers[1], 
             ProblemMarkers[stageDifficulty == Difficulty.hard ? 2 : 3]);
    }
    public void InstantiateGivenAdvance(Transform Lside, Transform Rside, Vector3 varSpawnPt, Vector3 prodSpawnPt, Vector3 constSpawnPt)
    {
        Debug.Log("Instantiating Advance Given");

        // Add SQR Variable
        GameObject v = Instantiate(sqrVariable, varSpawnPt, Quaternion.identity);
        v.transform.SetParent(Lside, false);
        v.GetComponent<Shapes>().ChangeToGiven();

        // Add Product
        GameObject p = Instantiate(product, prodSpawnPt, Quaternion.identity);
        p.transform.SetParent(Lside, false);
        p.GetComponent<Shapes>().ChangeToGiven();

        // Add Constant
        GameObject c = Instantiate(constant, constSpawnPt, Quaternion.identity);
        c.transform.SetParent(stageDifficulty == Difficulty.hard ? Lside : Rside, false);
        c.GetComponent<Shapes>().ChangeToGiven();

        // Add Value
        switch (stageDifficulty)
        {
            case Difficulty.easy:
                v.GetComponent<Shapes>().AddQuotientValue(1);
                p.GetComponent<Shapes>().AddQuotientValue(4);
                c.GetComponent<Shapes>().AddQuotientValue(2);
                break;

        }
        if (stageDifficulty != Difficulty.easy)
        {
            //List<int> randVal = new List<int>(){ 4, 9, 16 };
            v.GetComponent<Shapes>().AddQuotientValue(1); //randVal[Random.Range(0,randVal.Count)]
            varVal = v.GetComponent<Shapes>().value;
            List<int> randVal = new List<int>() { 4, 10, 16, 20 };
            p.GetComponent<Shapes>().AddQuotientValue(randVal[Random.Range(0, randVal.Count)]);
            randVal = new List<int>() { 2, 3, 4, 5,7,8 };
            c.GetComponent<Shapes>().AddQuotientValue(randVal[Random.Range(0, randVal.Count)]);
        }

        // get answer
        if (answers1.Count == 0)
        {
            var a = v.GetComponent<Shapes>().value;
            var b = p.GetComponent<Shapes>().value;
            var x = c.GetComponent<Shapes>().value;
            SolveForAnswer(a, b, x);
        }


    }
    public void SolveForAnswer(int a, int b, int c)
    {
        int ans = 0;
        ans = (int)Mathf.Sqrt(a);
        a = ans;
        answers1.Add(a);

        // (b/2)^2
        // Add last term
        ans = b / 2;
        b = ans;
        answers1.Add(b);


        ans = stageDifficulty == Difficulty.hard ? (0 - c) : c;
        Debug.Log(ans + " \t " + b);
        c = ans + (b * b);
        answers1.Add(c);

        answers2.Add(0 - b);
        SolveForRadical(c);

    }
    private void SolveForRadical(int n)
    {
        int outside = 1;
        int inside = n;

        // Find perfect square factors
        for (int i = Mathf.FloorToInt(Mathf.Sqrt(n)); i >= 2; i--)
        {
            int square = i * i;
            if (inside % square == 0)
            {
                outside *= i;
                inside /= square;
            }
        }

        answers.Clear();
        answers.Add(outside);
        answers.Add(inside);
    }
}
