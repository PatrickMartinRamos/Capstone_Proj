using NUnit.Framework;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class BinomiallProblemLoader : ProblemLoader
{
    [SerializeField] GameObject leftSide; // 0 = variable , 1 = symbol , 2 = constant
    [SerializeField] GameObject rightSide; // 0 = variable , 1 = symbol , 2 = constant
    [SerializeField] GameObject plusSymbol;
    [SerializeField] GameObject minusSymbol;
    private GameObject variable;
    private GameObject constant;
    private List<int> answers = new List<int>();

    private void Start()
    {
        StageManager.Instance.problem = this;
        variable = StageManager.Instance.variable;
        constant = StageManager.Instance.constant;
        //LoadProblem();
    }
    public override void LoadProblem()
    {
        GetLevelDifficulty();
        if (stageDifficulty != Difficulty.easy)
        {
            variable.GetComponent<Shapes>().AddValue(stageDifficulty);
            constant.GetComponent<Shapes>().AddValue(stageDifficulty);
        }
        InstantiateGiven(leftSide.transform, ProblemMarkers[0], ProblemMarkers[1], ProblemMarkers[2]);

        InstantiateGiven(rightSide.transform, ProblemMarkers[3], ProblemMarkers[4], ProblemMarkers[5]);

    }
    public void InstantiateGiven(Transform side, Vector3 varSpawnPt, Vector3 symSpawnPt, Vector3 conSpawnPt)
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
        if (answers.Count == 0)
        {
            var a = v.GetComponent<Shapes>().value;
            var b = v.GetComponent<Shapes>().value;
            SolveForAnswer(a, b);
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
    public override List<int> Answers()
    {
        return answers;
    }

}
