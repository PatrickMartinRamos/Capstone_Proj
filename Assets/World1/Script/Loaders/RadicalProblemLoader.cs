using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class RadicalProblemLoader : ProblemLoader
{
    [SerializeField] GameObject givenLocation;
    [SerializeField] GameObject scissorLocation;

    private GameObject constant;

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
        // Instantiate the given shape
        GameObject v = Instantiate(constant, Vector3.zero, Quaternion.identity);
        v.GetComponent<Shapes>().AddValue(stageDifficulty);
        v.transform.SetParent(parent, false);
        v.transform.localPosition = Vector3.zero;

        Shapes s = v.GetComponent<Shapes>();
        s.ChangeToGiven();
        s.valueLabel.GetComponent<TextMeshProUGUI>().text = s.value.ToString();
        s.valueLabel.SetActive(true);

        // Generate answer only once
        if (answers.Count == 0)
        {
            SolveForAnswer(s.value);
        }
    }

    private void SolveForAnswer(int n)
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
