using System.Collections.Generic;
using UnityEngine;

public class RadicalProblemLoader : ProblemLoader
{
    [SerializeField] GameObject givenLocation;
    [SerializeField] GameObject scissorLocation;
    private GameObject constant;
    private List<int> answers = new List<int>();

    void Start()
    {
        StageManager.Instance.problem = this;
        constant = StageManager.Instance.constant;
    }

    public override void LoadProblem()
    {
        GetLevelDifficulty();
        InstantiateGiven(givenLocation.transform);
    }

    public void InstantiateGiven(Transform parent)
    {
        // Instantiate the base shape
        GameObject v = Instantiate(constant, Vector3.zero, Quaternion.identity);
        v.transform.SetParent(parent, false);
        v.transform.localPosition = Vector3.zero;

        var shape = v.GetComponent<Shapes>();

        // Generate NON-PRIME VALUE FIRST
        int nonPrimeValue = GenerateNonPrimeValue(stageDifficulty);
        shape.value = nonPrimeValue; // Make sure value is assigned BEFORE ChangeToGiven()

        // Display value
        shape.valueLabel.SetActive(true);
        shape.valueLabel.GetComponent<TMPro.TextMeshProUGUI>().text = nonPrimeValue.ToString();

        shape.ChangeToGiven();

        // Calculate radical simplification only once
        if (answers.Count == 0)
        {
            SolveForAnswer(nonPrimeValue);
        }
    }

    // -------------------------
    //  NON-PRIME GENERATOR
    // -------------------------
    private int GenerateNonPrimeValue(Difficulty difficulty)
    {
        int min = 4;  // smallest non-prime
        int max = 20;

        if (difficulty == Difficulty.normal)
        {
            min = 4;
            max = 12;
        }
        else if (difficulty == Difficulty.hard)
        {
            min = 8;
            max = 30;
        }

        int value;
        do
        {
            value = Random.Range(min, max + 1);
        }
        while (IsPrime(value));

        return value;
    }

    // SIMPLE PRIME CHECKER
    private bool IsPrime(int n)
    {
        if (n <= 1) return false;
        if (n == 2) return true;
        if (n % 2 == 0) return false;

        int boundary = Mathf.FloorToInt(Mathf.Sqrt(n));
        for (int i = 3; i <= boundary; i += 2)
        {
            if (n % i == 0) return false;
        }

        return true;
    }

    // -------------------------
    //  RADICAL SIMPLIFIER
    // -------------------------
    private void SolveForAnswer(int n)
    {
        int outside = 1;
        int inside = n;

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
        answers.Add(outside);  // coefficient outside the radical
        answers.Add(inside);   // remaining inside the radical
    }
}
