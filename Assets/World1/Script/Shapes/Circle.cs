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
                break;

        }

        GameObject newShape = Instantiate(CombinedShape, spawnPt, Quaternion.identity);
        newShape.GetComponent<Shapes>().AddValue(dragged.GetComponent<Shapes>().value, target.GetComponent<Shapes>().value);
        newShape.GetComponent<Shapes>().MoveToArea();

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
}
