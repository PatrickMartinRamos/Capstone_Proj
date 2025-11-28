using UnityEngine;
using static UnityEngine.UI.Image;

public class Duplicator : Shapes
{
    public override void Interact(GameObject original)
    {

        GameObject duplicate = Instantiate(original, original.transform.parent);
        duplicate.transform.SetSiblingIndex(original.transform.GetSiblingIndex() + 1);

        if (StageManager.Instance.isAdvanceCTS && original.transform.parent.gameObject.name == "right")
        {
            duplicate.GetComponent<Shapes>().MoveToArea2();
        }
        else
            duplicate.GetComponent<Shapes>().MoveToArea();

        this.RevertPosition();

    }
}
