using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using static UnityEngine.UI.Image;

public class Duplicator : Shapes
{
    public override void Interact(GameObject original)
    {
        if (original.GetComponent<Shapes>().GetClassification() == ShapeClassification.Scissors) return;

        GameObject duplicate = Instantiate(original, original.transform.parent);
        duplicate.transform.SetSiblingIndex(original.transform.GetSiblingIndex() + 1);

        if (StageManager.Instance.isAdvanceCTS && original.transform.parent.gameObject.name == "Right")
        {
            duplicate.GetComponent<Shapes>().MoveToArea2();
        }
        else 
            duplicate.GetComponent<Shapes>().MoveToArea(!original.GetComponent<Shapes>().isGiven ? original.transform.parent : null) ;

        this.RevertPosition();

    }
}
