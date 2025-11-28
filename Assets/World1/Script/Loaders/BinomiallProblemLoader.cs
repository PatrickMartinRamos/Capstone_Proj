using NUnit.Framework;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class BinomiallProblemLoader : ProblemLoader
{
    [SerializeField] GameObject leftSide; // 0 = variable , 1 = symbol , 2 = constant
    [SerializeField] GameObject rightSide; // 0 = variable , 1 = symbol , 2 = constant
    [SerializeField] GameObject plusSymbol;
    [SerializeField] GameObject minusSymbol;
    private GameObject variable;
    private GameObject constant;

    protected override void Start()
    {
        StageManager.Instance.problem = this;
        variable = StageManager.Instance.variable;
        constant = StageManager.Instance.constant;
        //LoadProblem();
    }
    public override void LoadProblem()
    {
        GetLevelDifficulty();

        int vValue = 1;
        int cValue = 1;

        switch (stageDifficulty)
        {
            case Difficulty.easy:
                break;
            case Difficulty.normal:
                vValue = Random.Range(2, 3);
                cValue = Random.Range(2, 5);
                break;
            case Difficulty.hard:
                vValue = Random.Range(3, 5);
                cValue = Random.Range(5, 10);
                break;
            default:
                break;

        }


        InstantiateGiven(leftSide.transform, vValue, cValue, ProblemMarkers[0], ProblemMarkers[1], ProblemMarkers[2]);
        InstantiateGiven(rightSide.transform, vValue, cValue, ProblemMarkers[3], ProblemMarkers[4], ProblemMarkers[5]);


        if (answers.Count == 0)
        {
            var a = vValue;
            var b = cValue;
            SolveForAnswer(a, b);
        }

    }
    public void InstantiateGiven(Transform side, int vVal, int cVal, Vector3 varSpawnPt, Vector3 symSpawnPt, Vector3 conSpawnPt)
    {

        // Add Variable
        GameObject v = Instantiate(variable, varSpawnPt, Quaternion.identity);
        v.transform.SetParent(side, false);
        v.GetComponent<Shapes>().ChangeToGiven();

        // Add Symbol
        Instantiate(plusSymbol, symSpawnPt, Quaternion.identity).transform.SetParent(side, false);

        // Add Constant
        GameObject c = Instantiate(constant, conSpawnPt, Quaternion.identity);
        c.transform.SetParent(side, false);
        c.GetComponent<Shapes>().ChangeToGiven();

        // Add Value
        if (stageDifficulty != Difficulty.easy)
        {
            v.GetComponent<Shapes>().AddQuotientValue(vVal);
            c.GetComponent<Shapes>().AddQuotientValue(cVal);
        }

    }
    public void SolveForAnswer(int a, int b)
    {
        // a^2 + 2ab + b^2
        // Add first term
        answers.Add(a*a);
        // Add second term
        answers.Add(2 * a * b);
        // Add third term
        answers.Add(b*b);
    }

}
