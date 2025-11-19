using UnityEngine;

public class CraftAreaMech : MonoBehaviour, ITargetable
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnEnable()
    {
        StageManager.Instance.craftArea = this.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void InteractWithDraggedObject(GameObject draggedObject)
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
