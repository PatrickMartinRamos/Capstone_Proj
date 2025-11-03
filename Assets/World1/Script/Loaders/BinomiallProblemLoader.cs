using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class BinomiallProblemLoader : ProblemLoader
{
    [SerializeField] GameObject leftSide; // 0 = variable , 1 = symbol , 2 = constant
    [SerializeField] GameObject rightSide; // 0 = variable , 1 = symbol , 2 = constant
    [SerializeField] GameObject plusSymbol;
    [SerializeField] GameObject minusSymbol;
    private GameObject variable;
    private GameObject constant;

    private void Start()
    {
        variable = StageManager.Instance.variable;
        constant = StageManager.Instance.constant;
        LoadProblem();
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

        // Add Symbol
        Instantiate(GetSymbol(stageDifficulty), symSpawnPt, Quaternion.identity).transform.SetParent(side, false);

        // Add Constant
        GameObject c = Instantiate(variable, conSpawnPt, Quaternion.identity);
        c.transform.SetParent(side, false);
    }
    private void GetLevelDifficulty()
    {
        int level = PlayerPrefs.GetInt("StageID");
        if (level != 0)
        {
            int levelIndicator = level % 3;
            switch (levelIndicator)
            {
                case 0:
                    stageDifficulty = Difficulty.easy;
                    break;
                case 1:
                    stageDifficulty = Difficulty.normal;
                    break;
                case 2:
                    stageDifficulty = Difficulty.hard;
                    break;
            }
        }
    }
    private GameObject GetSymbol(Difficulty difficulty)
    {
        switch (stageDifficulty)
        {
            case Difficulty.easy:
                return plusSymbol;
            case Difficulty.normal:
                return minusSymbol;
            case Difficulty.hard:
                int roll = Random.Range(0, 2);
                return roll == 0 ? plusSymbol : minusSymbol;
        }
        return null;
    }
}
