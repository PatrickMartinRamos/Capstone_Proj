using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class CompletingSquareProblemLoader : ProblemLoader
{
    [SerializeField] GameObject leftSide; // 0 = variable , 1 = product
    [SerializeField] GameObject plusSymbol;
    private GameObject variable;
    private GameObject product;
    private int varVal = 0;
    public int VarValue => varVal;
    private void Start()
    {
        StageManager.Instance.problem = this;
        variable = StageManager.Instance.variable;
        product = StageManager.Instance.product;
        //LoadProblem();
    }
    public override void LoadProblem()
    {
        GetLevelDifficulty();
        InstantiateGiven(leftSide.transform, ProblemMarkers[0], ProblemMarkers[1]);
    }
    public void InstantiateGiven(Transform side, Vector3 varSpawnPt, Vector3 prodSpawnPt)
    {
        Debug.Log("Instantiating Given");

        // Add Variable
        GameObject v = Instantiate(variable, varSpawnPt, Quaternion.identity);
        v.transform.SetParent(side, false);
        v.GetComponent<Shapes>().ChangeToGiven();

        // Add Constant
        GameObject p = Instantiate(product, prodSpawnPt, Quaternion.identity);
        p.transform.SetParent(side, false);
        p.GetComponent<Shapes>().ChangeToGiven();

        // Add Value
        switch(stageDifficulty)
        {
            case Difficulty.easy:
                v.GetComponent<Shapes>().AddQuotientValue(1);
                p.GetComponent<Shapes>().AddQuotientValue(4);
                break;

        }
        if (stageDifficulty != Difficulty.easy)
        {
            //List<int> randVal = new List<int>(){ 4, 9, 16 };
            v.GetComponent<Shapes>().AddQuotientValue(1); //randVal[Random.Range(0,randVal.Count)]
            varVal = v.GetComponent<Shapes>().value;
            List<int> randVal = new List<int>(){ 4, 10, 16, 20 };
            p.GetComponent<Shapes>().AddQuotientValue(randVal[Random.Range(0, randVal.Count)]);
        }

        // get answer
        if (answers.Count == 0)
        {
            var a = v.GetComponent<Shapes>().value;
            var b = p.GetComponent<Shapes>().value;
            SolveForAnswer(b);
        }


    }
    public void SolveForAnswer(int b)
    {
        // (b/2)^2
        // Add last term
        int ans = b / 2;
        b = ans * ans;
        answers.Add(b);

    }

}
