using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class VerifierMechanics : MonoBehaviour, ITargetable
{
    [SerializeField] private GameObject embedShape;
    [SerializeField] private ShapeClassification correctShape;
    [SerializeField] private SpriteRenderer verificationIndicator;
    [SerializeField] Color correct, normal, wrong;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    private void OnTransformChildrenChanged()
    {
        if (embedShape.transform.parent == this.gameObject.transform)
            return;
        resetColorIndicator();
        embedShape = null;
    }
    public void InteractWithDraggedObject(GameObject draggedObject)
    {
        if (embedShape  != null)
        {
            StageManager.Instance.NotificationText.text = "A Gear is Already Embed. Remove first to proceed.";
            return;
        } 

        if (draggedObject.transform.parent != this)
        {
            embedShape = draggedObject;
            Debug.Log("Dragged Into: Verifier" + " \nDragged Object: " + this.gameObject.name);
            draggedObject.transform.SetParent(transform,false);
        }
        draggedObject.transform.localPosition = Vector3.zero;
        verificationIndicator.color = Color.yellow;
        draggedObject.GetComponent<Shapes>().FixScale(2f);
    }
    public bool VerifyShape(int answer)
    {
        int submittedAns = 0;
        ShapeClassification embedShapeClass;

        embedShapeClass = embedShape != null ? embedShape.GetComponent<Shapes>().GetClassification() : ShapeClassification.Null;
        submittedAns = embedShape != null ? embedShape.GetComponent<Shapes>().value : 0;
        if (embedShapeClass == correctShape)
        {
            if(StageManager.Instance.bombsManager.targetBomb.GetComponent<BombMechanics>().problemType == ProblemType.radicals 
                && StageManager.Instance.problem.stageDifficulty == Difficulty.easy)
            {
                verificationIndicator.color = correct;
                return true;
            }
            if (submittedAns == answer)
            {
                Debug.Log("All Correct.");

                verificationIndicator.color = correct;
                return true;
            }
            else
            {
                Debug.Log("Incorrect Value\t" +submittedAns);

                verificationIndicator.color = wrong;
                return false;
            }

        }
        else
        {
            Debug.Log("Incorrect Gear\t" + embedShapeClass);
            verificationIndicator.color = wrong;
            return false;
        }

    }
    public void resetColorIndicator()
    {
        verificationIndicator.color = normal;
    }
}
