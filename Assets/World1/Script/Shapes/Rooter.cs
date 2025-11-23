using DG.Tweening;
using System.Collections;
using UnityEngine;

public class Rooter : Scissors
{
    public override void CombineShapes(GameObject dragged, GameObject target)
    {
        int val = (int)Mathf.Sqrt(target.GetComponent<Shapes>().value);
        ;
        // Get Midpoint of 2 shapes
        Vector2 spawnPt = (target.transform.position + dragged.transform.position) / 2;
        ShapeClassification targetClass = target.GetComponent<Shapes>().GetClassification();
        Debug.Log("Target Classification = " + targetClass);
        switch (targetClass)
        {
            case ShapeClassification.DoubleCircle:
                CombinedShape1 = StageManager.Instance.constant;
                break;

            case ShapeClassification.Circle:
                int value = dragged.GetComponent<Shapes>().value;
                if (value < 0)
                {
                    StageManager.Instance.NotificationText.text = "Gear has no square root.";
                    return;
                }
                else if (val * val == value)
                {
                    Debug.Log(value + " = " + val*val);
                    CombinedShape1 = StageManager.Instance.constant;
                    break;
                }
                else
                {
                    return;
                }

            case ShapeClassification.DoubleSquare:
                CombinedShape1 = StageManager.Instance.variable;
                break;

            default:
                StageManager.Instance.NotificationText.text = "Gear Cannot be Factorized.";
                return;
            }

        GameObject newShape = Instantiate(CombinedShape1, spawnPt, Quaternion.identity);
        newShape.GetComponent<Shapes>().AddQuotientValue(val);
        newShape.GetComponent<Shapes>().MoveToArea(this.transform.parent);
        target.SetActive(false);


    }

    protected override void SetPos()
    {
        ChangeOriginPos(new Vector3(-2, -2.5f, 0));
    }

}
