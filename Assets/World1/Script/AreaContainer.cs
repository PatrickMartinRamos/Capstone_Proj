using UnityEngine;

public class AreaContainer : MonoBehaviour, ITargetable
{
    public void InteractWithDraggedObject(GameObject draggedObject)
    {
        AreaIntegration(draggedObject);
    }
    protected virtual void AreaIntegration(GameObject draggedObject)
    {
        if (StageManager.Instance.ActiveGameArea.GetComponent<BinomiallProblemLoader>() && (draggedObject.GetComponent<Shapes>().GetClassification() == ShapeClassification.Square
            || draggedObject.GetComponent<Shapes>().GetClassification() == ShapeClassification.Circle))
        {
            draggedObject.GetComponent<Shapes>().returnToInitParent();
            return;
        }
        else if (StageManager.Instance.ActiveGameArea.GetComponent<CompletingSquareProblemLoader>() && (draggedObject.GetComponent<Shapes>().GetClassification() == ShapeClassification.DoubleSquare
    || draggedObject.GetComponent<Shapes>().GetClassification() == ShapeClassification.Triangle))
        {
            draggedObject.GetComponent<Shapes>().returnToInitParent();
            return;
        }
        else if (draggedObject.GetComponent<Shapes>().GetClassification() == ShapeClassification.Scissors || draggedObject.gameObject.name == "Rooter" || draggedObject.gameObject.name == "Duplicator")
            return;

        if (draggedObject.transform.parent != this.gameObject.transform )
        {
            if(draggedObject.transform.parent.GetComponent<CraftAreaMech>()!= null && this.gameObject.GetComponent<CraftAreaMech>()!=null)
                draggedObject.GetComponent<Shapes>().ReverseValue();

            draggedObject.transform.SetParent(transform, true);

        }

        draggedObject.GetComponent<Shapes>().FixScale();
    }
}
