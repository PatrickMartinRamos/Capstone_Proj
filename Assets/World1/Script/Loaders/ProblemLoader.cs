using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class ProblemLoader : MonoBehaviour
{
    [SerializeField] internal List<Vector3> ProblemMarkers;
    internal Difficulty stageDifficulty;

    public virtual void LoadProblem()
    {

    }
}
