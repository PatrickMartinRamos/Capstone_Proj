using System.Collections.Generic;
using UnityEngine;

public class RadicalProblemLoader : ProblemLoader
{
    [SerializeField] GameObject givenLocation;
    [SerializeField] GameObject scissorLocation;
    private GameObject constant;
    private List<int> answers = new List<int>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StageManager.Instance.problem = this;
        constant = StageManager.Instance.constant;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void LoadProblem()
    {
        GetLevelDifficulty();
        constant.GetComponent<Shapes>().AddValue(stageDifficulty);
        InstantiateGiven(givenLocation.transform);

    }

    public void InstantiateGiven(Transform parent)
    {
        // Add Constant Given
        GameObject v = Instantiate(constant, Vector3.zero, Quaternion.identity);
        v.transform.SetParent(parent, false);
        v.transform.localPosition = Vector3.zero;
        v.GetComponent<Shapes>().ChangeToGiven();
        v.GetComponent <Shapes>().valueLabel.SetActive(true);


        if (answers.Count == 0)
        {
            var a = v.GetComponent<Shapes>().value;
            SolveForAnswer(a);
        }


    }
    private void SolveForAnswer(int n)
    {
        int outside = 1;
        int inside = n;

        // Loop down from √n to find perfect square factors
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
        answers.Add(outside);   // coefficient outside √
        answers.Add(inside);    // remaining inside √
    }
}
