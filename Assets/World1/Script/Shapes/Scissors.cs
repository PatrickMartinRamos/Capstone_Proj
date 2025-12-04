using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Scissors : Shapes
{
    protected bool isPosChangeable = true;
    protected override void Start()
    {
        ChangeOriginPos(new Vector3(3, 3, 0));
        isPosChangeable = false;
        this.RevertPosition();
    }
    public GameObject CombinedShape1, CombinedShape2;
    public override void ChangeOriginPos(Vector3 newPos)
    {
        if (isPosChangeable)
        {
            originPos = newPos;
        }
        else return;
    }
    
    public override void CombineShapes(GameObject dragged, GameObject target)
    {
        int val1 = 0;
        int val2 = 0;
        // Get Midpoint of 2 shapes
        Vector2 spawnPt = (target.transform.position + dragged.transform.position) / 2;
        ShapeClassification targetClass = target.GetComponent<Shapes>().GetClassification();
        Debug.Log("Target Classification = " + targetClass);
        switch (targetClass)
        {
            case ShapeClassification.DoubleCircle:
                CombinedShape1 = StageManager.Instance.constant;
                val1 = Mathf.RoundToInt(Mathf.Sqrt(target.GetComponent<Shapes>().value));
                CombinedShape2 = StageManager.Instance.constant;
                val2 = Mathf.RoundToInt(Mathf.Sqrt(target.GetComponent<Shapes>().value));
                break;
            case ShapeClassification.Circle:
                if (StageManager.Instance.bombsManager.targetBomb.GetComponent<BombMechanics>().problemType == ProblemType.completingSquare || StageManager.Instance.bombsManager.targetBomb.GetComponent<BombMechanics>().problemType == ProblemType.advanceCompletingSquare)
                {
                    int targetValue = target.GetComponent<Shapes>().value;
                    GetFactorPair(targetValue, 2, out val1, out val2);

                    CombinedShape1 = StageManager.Instance.constant;
                    CombinedShape2 = StageManager.Instance.constant;
                }

                else if (StageManager.Instance.problem.stageDifficulty == Difficulty.easy)
                {
                    int targetValue = target.GetComponent<Shapes>().value;
                    GetFactorPair(targetValue, out val1, out val2);
                    int root = Mathf.RoundToInt(Mathf.Sqrt(val1));

                    CombinedShape1 = root * root == val1 ? StageManager.Instance.squaredConstant : StageManager.Instance.constant;

                    root = Mathf.RoundToInt(Mathf.Sqrt(val2));

                    CombinedShape2 = root * root == val2 ? StageManager.Instance.squaredConstant : StageManager.Instance.constant;
                }
                else
                {
                    int targetValue = target.GetComponent<Shapes>().value;
                    GetFactorPair(targetValue, out val1, out val2);

                    int root = Mathf.RoundToInt(Mathf.Sqrt(val1));

                    CombinedShape1 = root * root == val1 ? StageManager.Instance.squaredConstant : StageManager.Instance.constant;

                    root = Mathf.RoundToInt(Mathf.Sqrt(val2));

                    CombinedShape2 = root * root == val2 ? StageManager.Instance.squaredConstant : StageManager.Instance.constant;
                }
                break;
            case ShapeClassification.Triangle:
                if (StageManager.Instance.bombsManager.targetBomb.GetComponent<BombMechanics>().problemType == ProblemType.squaringBinomial || StageManager.Instance.bombsManager.targetBomb.GetComponent<BombMechanics>().problemType == ProblemType.radicals)
                    return;
                int varVal = StageManager.Instance.problem.gameObject.GetComponent<CompletingSquareProblemLoader>().VarValue;
                CombinedShape1 = StageManager.Instance.variable;
                val1 = 1;
                CombinedShape2 = StageManager.Instance.constant;
                val2 = target.GetComponent<Shapes>().value;
                break;
            case ShapeClassification.DoubleSquare:
                CombinedShape1 = StageManager.Instance.variable;
                val1 = (int)Mathf.Sqrt(target.GetComponent<Shapes>().value);
                CombinedShape2 = StageManager.Instance.variable;
                val2 = (int)Mathf.Sqrt(target.GetComponent<Shapes>().value);
                break;

            default:
                StageManager.Instance.NotificationText.text = "Gear Cannot be Factorized.";
                return;
        }

        StartCoroutine(InstantiateQuotients(dragged, target, val1, val2));


    }
/*    public override void AddValue(Difficulty difficulty)
    {
        switch (difficulty)
        {
            case Difficulty.normal:
                value = 1;
                valueLabel.GetComponent<TextMeshProUGUI>().text = value.ToString();
                break;
            case Difficulty.hard:
                int stageLevel = PlayerPrefs.GetInt("StageID");
                value = stageLevel < 7 ? Random.Range(2, 5) : Random.Range(6, 10);
                valueLabel.GetComponent<TextMeshProUGUI>().text = value.ToString();
                valueLabel.SetActive(true);
                break;
        }
        base.AddValue(difficulty);
    }*/
    IEnumerator InstantiateQuotients(GameObject dragged, GameObject target, int val1, int val2) 
    {
        GameObject newShape = null;

        if (StageManager.Instance.isAdvanceCTS )
        {

            if (target.GetComponent<Shapes>().isGiven)
            {
                newShape = Instantiate(CombinedShape1, spawnPt, Quaternion.identity, StageManager.Instance.craftArea2.transform);
                newShape.GetComponent<Shapes>().AddQuotientValue(val1);
                newShape.GetComponent<Shapes>().MoveToArea2();
                yield return new WaitForSeconds(0.5f);

                newShape = Instantiate(CombinedShape2, spawnPt, Quaternion.identity, StageManager.Instance.craftArea2.transform);
                newShape.GetComponent<Shapes>().AddQuotientValue(val2);
                newShape.GetComponent<Shapes>().MoveToArea2();
            }
            //var spawnTrans = target.transform.parent.GetComponent<CraftAreaMech>() ? target.transform : null;
            newShape = Instantiate(CombinedShape1, spawnPt, Quaternion.identity, StageManager.Instance.craftArea.transform);
            newShape.GetComponent<Shapes>().AddQuotientValue(val1);
            newShape.GetComponent<Shapes>().MoveToArea(!target.GetComponent<Shapes>().isGiven ? target.transform.parent : null);
            yield return new WaitForSeconds(0.5f);

            newShape = Instantiate(CombinedShape2, spawnPt, Quaternion.identity, StageManager.Instance.craftArea.transform);
            newShape.GetComponent<Shapes>().AddQuotientValue(val2);
            newShape.GetComponent<Shapes>().MoveToArea(!target.GetComponent<Shapes>().isGiven ? target.transform.parent : null);

        }
        else
        {
            newShape = Instantiate(CombinedShape1, spawnPt, Quaternion.identity);
            newShape.GetComponent<Shapes>().AddQuotientValue(val1);
            newShape.GetComponent<Shapes>().MoveToArea();
            yield return new WaitForSeconds(0.5f);

            newShape = Instantiate(CombinedShape2, spawnPt, Quaternion.identity);
            newShape.GetComponent<Shapes>().AddQuotientValue(val2);
            newShape.GetComponent<Shapes>().MoveToArea();
        }
        //target.SetActive(false);
    }
    private void GetFactorPair(int value, out int val1, out int val2)
    {
        // 1. Check for perfect square
        int root = (int)Mathf.Sqrt(value);
        if (root * root == value)
        {
            val1 = root;
            val2 = root;
            return;
        }

        // 2. Find any factor pair (prefer the one closest to square root)
        val1 = 1;
        val2 = value;

        for (int i = root; i >= 1; i--)
        {
            if (value % i == 0)
            {
                val1 = i;
                val2 = value / i;
                return;
            }
        }
    }
    private void GetFactorPair(int value, int divisor, out int a, out int b)
    {
        a = value / divisor;
        b = 2;
        return;
    }


}
