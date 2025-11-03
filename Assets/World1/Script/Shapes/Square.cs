using UnityEditor;
using UnityEngine;

public class Square : Shapes
{
    public GameObject CombinedShape;
    public override void CombineShapes(GameObject dragged, GameObject target)
    {
        Debug.Log(dragged.name + " \n" + target.name);
        // Get Midpoint of 2 shapes
        Vector2 spawnPt = (target.transform.position + dragged.transform.position)/2;
        ShapeClassification targetClass = target.GetComponent<Shapes>().GetClassification();
        Debug.Log("Target Classification = " + targetClass);
        switch(targetClass)
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
        Instantiate(CombinedShape, spawnPt, Quaternion.identity);

    }
    public override void AddValue(Difficulty difficulty)
    {
        base.AddValue(difficulty);
        switch (difficulty)
        {
            case Difficulty.normal:
                value = 1;
                break;
            case Difficulty.hard:
                int stageLevel = PlayerPrefs.GetInt("StageID");
                value = stageLevel < 7 ? Random.Range(2,5) : Random.Range(6,12);
                break;
        }
    }
}
