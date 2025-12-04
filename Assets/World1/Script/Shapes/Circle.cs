/*using TMPro;
using UnityEngine;

public class Circle : UIShape
{
    public GameObject CombinedShapePrefab;
    private void Start()
    {
        base.Start();
        shapeName = "Constant";
    }
    public override void CombineShapes(GameObject dragged, GameObject target)
    {
        Debug.Log("Combining Different Shapes...");
        UIShape draggedShape = dragged.GetComponent<UIShape>();
        UIShape targetShape = target.GetComponent<UIShape>();

        if (draggedShape == null || targetShape == null)
        {
            Debug.Log($"{dragged} and {target}");
            return;
        }

        // Midpoint using world space
        Vector3 midpoint = (dragged.transform.position + target.transform.position) * 0.5f;

        ShapeClassification targetClass = targetShape.GetClassification();
        Debug.Log("Target Classification = " + targetClass);

        // Pick UI prefab depending on classification
        switch (targetClass)
        {
            case ShapeClassification.Square:
                CombinedShapePrefab = StageManager.Instance.product;              // UI version
                break;

            case ShapeClassification.Circle:
                CombinedShapePrefab = StageManager.Instance.squaredConstant;     // UI version
                break;

            default:
                Debug.LogWarning("Cannot combine shapes");
                return;
        }

        // Instantiate inside same UI parent
        Debug.Log("Instantiating New Shapes...");

        GameObject newShapeObj = Instantiate(
            CombinedShapePrefab,
            target.transform.parent // IMPORTANT: UI parenting
        );

        // Position new UI object at midpoint
        RectTransform newRect = newShapeObj.GetComponent<RectTransform>();
        newRect.position = midpoint;

        UIShape newShape = newShapeObj.GetComponent<UIShape>();

        // Combine values (value * value or however your logic works)
        newShape.AddValue(
            draggedShape.value,
            targetShape.value
        );

        // Move to correct UI area or apply any UI animation
        newShape.MoveToArea();
    }

    public override void AddValue(Difficulty difficulty)
    {
        switch (difficulty)
        {
            case Difficulty.normal:
                value = Random.Range(1, 10);
                valueLabel.GetComponent<TextMeshProUGUI>().text = value.ToString();
                break;

            case Difficulty.hard:
                value = Random.Range(6, 15);
                valueLabel.GetComponent<TextMeshProUGUI>().text = value.ToString();
                valueLabel.SetActive(true);
                break;
        }

        base.AddValue(difficulty);
    }
}*/

using TMPro;
using UnityEngine;

public class Circle : Shapes
{
    public GameObject CombinedShape;
    public override void CombineShapes(GameObject dragged, GameObject target)
    {
        // Get Midpoint of 2 shapes
        Vector2 spawnPt = (target.transform.position + dragged.transform.position) / 2;
        ShapeClassification targetClass = target.GetComponent<Shapes>().GetClassification();
        Debug.Log("Target Classification = " + targetClass);
        switch (targetClass)
        {
            case ShapeClassification.Square:
                CombinedShape = StageManager.Instance.product;
                break;
            case ShapeClassification.Circle:
                CombinedShape = StageManager.Instance.squaredConstant;
                break;
            default:
                Debug.Log("cannot combine shapes");
                return;

        }

        GameObject newShape = Instantiate(CombinedShape, spawnPt, Quaternion.identity);
        newShape.GetComponent<Shapes>().AddValue(dragged.GetComponent<Shapes>().value, target.GetComponent<Shapes>().value);
        newShape.GetComponent<Shapes>().MoveToArea();

    }
    public override void AddValue(Difficulty difficulty)
    {
        if(StageManager.Instance.bombsManager.targetBomb.GetComponent<BombMechanics>().problemType == ProblemType.squaringBinomial)
        {
            switch (difficulty)
            {
                case Difficulty.normal:
                    value = Random.Range(1, 10);
                    valueLabel.GetComponent<TextMeshProUGUI>().text = value.ToString();
                    break;
                case Difficulty.hard:
                    value = Random.Range(6, 15);
                    valueLabel.GetComponent<TextMeshProUGUI>().text = value.ToString();
                    valueLabel.SetActive(true);
                    break;
            }
            base.AddValue(difficulty);
        }
        else if (StageManager.Instance.bombsManager.targetBomb.GetComponent<BombMechanics>().problemType
                 == ProblemType.radicals)
        {
            switch (difficulty)
            {
                case Difficulty.easy:
                    value = 8;
                    valueLabel.GetComponent<TextMeshProUGUI>().text = value.ToString();
                    valueLabel.SetActive(true);
                    break;

                case Difficulty.normal:
                    value = GetRandomNonPrime(10, 30);   
                    valueLabel.GetComponent<TextMeshProUGUI>().text = value.ToString();
                    valueLabel.SetActive(true);
                    break;

                case Difficulty.hard:
                    value = GetRandomNonPrime(30, 80);   
                    valueLabel.GetComponent<TextMeshProUGUI>().text = value.ToString();
                    valueLabel.SetActive(true);
                    break;
            }

            base.AddValue(difficulty);
        }

    }
    private int GetRandomNonPrime(int min, int max) // max is exclusive
    {
        int num;

        do
        {
            num = Random.Range(min, max);
        }
        while (IsPrime(num));

        return num;
    }

    private bool IsPrime(int n)
    {
        if (n <= 1) return false;          // 0,1,negative are non-prime
        if (n == 2) return true;

        // Even numbers >2 are non-prime
        if (n % 2 == 0) return false;

        int boundary = Mathf.FloorToInt(Mathf.Sqrt(n));
        for (int i = 3; i <= boundary; i += 2)
        {
            if (n % i == 0)
                return false;
        }

        return true;
    }

    public override void CombineLikeTerms(GameObject dragged, GameObject target)
    {
        if(StageManager.Instance.problemType == ProblemType.radicals)
        {
            if (dragged.GetComponent<Shapes>().GetClassification() == ShapeClassification.Scissors || target.GetComponent<Shapes>().GetClassification() == ShapeClassification.Scissors) return;


            if (StageManager.Instance.problemType == ProblemType.completingSquare || StageManager.Instance.problemType == ProblemType.advanceCompletingSquare || StageManager.Instance.problemType == ProblemType.radicals)
            {
                GameObject newShape = null;
                if (target.GetComponent<Shapes>().GetClassification() == dragged.GetComponent<Shapes>().GetClassification())
                {
                    Debug.Log("Same Classification. Combining Like Terms");
                    if (dragged.GetComponent<Shapes>().GetClassification() == ShapeClassification.Circle)
                    {
                        if (dragged.GetComponent<Shapes>().value == target.GetComponent<Shapes>().value)
                            newShape = Instantiate(StageManager.Instance.squaredConstant, target.transform.position, Quaternion.identity);
                        else
                            newShape = Instantiate(StageManager.Instance.constant, target.transform.position, Quaternion.identity);
                    }

                    newShape.GetComponent<Shapes>().AddValue(target.GetComponent<Shapes>().value, dragged.GetComponent<Shapes>().value);

                    Transform t = target.transform.parent.GetComponent<CraftAreaMech>() == null ? StageManager.Instance.craftArea.transform : target.transform.parent;
                    Debug.Log(t.name);
                    newShape.GetComponent<Shapes>().MoveToArea(t);

                    Destroy(dragged);
                    Destroy(target);
                }

                else if ((dragged.GetComponent<Shapes>().GetClassification() == ShapeClassification.Circle ||
                    dragged.GetComponent<Shapes>().GetClassification() == ShapeClassification.DoubleCircle)
                    && (target.GetComponent<Shapes>().GetClassification() == ShapeClassification.Circle || target.GetComponent<Shapes>().GetClassification() == ShapeClassification.DoubleCircle))
                {
                    newShape = Instantiate(StageManager.Instance.constant, target.transform.position, Quaternion.identity);
                    newShape.transform.SetParent(target.transform.parent);

                    int a = dragged.GetComponent<Shapes>().value;
                    int b = target.GetComponent<Shapes>().value;

                    newShape.GetComponent<Shapes>().AddQuotientValue(a + b);

                    Destroy(dragged);
                    Destroy(target);
                }

            }
            else if (target.GetComponent<Shapes>().GetClassification() != dragged.GetComponent<Shapes>().GetClassification())
                return;

            target.GetComponent<Shapes>().AddValue(dragged.GetComponent<Shapes>().value);
            Destroy(dragged);
        }
        else
            base.CombineLikeTerms(dragged, target);
    }


}
