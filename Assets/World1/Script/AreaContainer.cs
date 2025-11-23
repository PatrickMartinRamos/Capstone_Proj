using UnityEngine;

public class AreaContainer : MonoBehaviour, ITargetable
{
    public void InteractWithDraggedObject(GameObject draggedObject)
    {
        AreaIntegration(draggedObject);
    }
    protected void AreaIntegration(GameObject draggedObject)
    {
        if (StageManager.Instance.ActiveGameArea.GetComponent<BinomiallProblemLoader>() && (draggedObject.GetComponent<Shapes>().GetClassification() == ShapeClassification.Square
            || draggedObject.GetComponent<Shapes>().GetClassification() == ShapeClassification.Circle))
        {
            draggedObject.GetComponent<Shapes>().returnToInitParent();
            return;
        }
        else if (draggedObject.GetComponent<Shapes>().GetClassification() == ShapeClassification.Scissors)
            return;

        if (draggedObject.transform.parent != this)
            draggedObject.transform.SetParent(transform, true);

        draggedObject.GetComponent<Shapes>().FixScale();
    }
}
