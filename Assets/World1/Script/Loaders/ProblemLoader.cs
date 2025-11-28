using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class ProblemLoader : MonoBehaviour
{
    [SerializeField] internal List<Vector3> ProblemMarkers;
    internal Difficulty stageDifficulty;
    protected List<int> answers = new List<int>();

    protected virtual void Start()
    {

    }
    public virtual void LoadProblem()
    {

    }
    public virtual List<int> Answers()
    {
        return answers;
    }
    internal void GetLevelDifficulty()
    {
        int level = StageManager.Instance.StageNumber;
        if (level != 0)
        {
            int levelIndicator = level % 3;
            switch (levelIndicator)
            {
                case 0:
                    stageDifficulty = Difficulty.hard;
                    break;
                case 1:
                    stageDifficulty = Difficulty.easy;
                    break;
                case 2:
                    stageDifficulty = Difficulty.normal;
                    break;
            }
        }
    }
}
