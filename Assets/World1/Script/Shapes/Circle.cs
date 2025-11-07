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
        newShape.GetComponent<Shapes>().MoveToArea();

    }
    public override void AddValue(Difficulty difficulty)
    {
        switch (difficulty)
        {
            case Difficulty.normal:
                value = Random.Range(1,10);
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
