/*using TMPro;
using UnityEngine;

public class Square : UIShape
{
    public GameObject CombinedShapePrefab;

    internal override void Start()
    {
        base.Start();
        shapeName = "Variable";
    }
    public override void CombineLikeTerms(GameObject dragged, GameObject target)
    {
        // UI version uses UIShape everywhere
        UIShape draggedShape = dragged.GetComponent<UIShape>();
        UIShape targetShape = target.GetComponent<UIShape>();

        if (draggedShape == null || targetShape == null)
            return;

        // Call base (adds value + destroys dragged)
        base.CombineLikeTerms(dragged, target);
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

        // Midpoint (world space)
        Vector3 midpoint = (dragged.transform.position + target.transform.position) * 0.5f;

        // Determine prefab based on classification
        ShapeClassification targetClass = targetShape.GetClassification();

        switch (targetClass)
        {
            case ShapeClassification.Square:
                CombinedShapePrefab = StageManager.Instance.squaredVariable;
                break;

            case ShapeClassification.Circle:
                CombinedShapePrefab = StageManager.Instance.product;
                break;

            default:
                Debug.LogWarning("Cannot combine shapes of this type.");
                return;
        }

        // Instantiate inside same UI canvas area
        Debug.Log("Instantiating new shape...");

        GameObject newShapeObj = Instantiate(
            CombinedShapePrefab,
            target.transform.parent // important for UI shapes
        );

        RectTransform newRect = newShapeObj.GetComponent<RectTransform>();
        newRect.position = midpoint;

        UIShape newShape = newShapeObj.GetComponent<UIShape>();

        // Example: multiply or combine values
        newShape.AddValue(
            draggedShape.value,
            targetShape.value
        );

        // If UI version uses snapping or animation, call UI move method
        newShape.MoveToArea();
    }

    public override void AddValue(Difficulty difficulty)
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
    }
}*/

using TMPro;
using UnityEditor;
using UnityEngine;

public class Square : Shapes
{
    public GameObject CombinedShape;
    public override void CombineLikeTerms(GameObject dragged, GameObject target)
    {
        base.CombineLikeTerms(dragged, target);
    }
    public override void CombineShapes(GameObject dragged, GameObject target)
    {
        // Get Midpoint of 2 shapes
        Vector2 spawnPt = (target.transform.position + dragged.transform.position) / 2;
        ShapeClassification targetClass = target.GetComponent<Shapes>().GetClassification();
        Debug.Log("Target Classification = " + targetClass);
        switch (targetClass)
        {
            case ShapeClassification.Square:
                CombinedShape = StageManager.Instance.squaredVariable;
                break;
            case ShapeClassification.Circle:
                CombinedShape = StageManager.Instance.product;
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

    }
}
