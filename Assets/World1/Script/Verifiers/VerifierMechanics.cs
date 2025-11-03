using UnityEngine;

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

    // Update is called once per frame
    void Update()
    {
        
    }
    public void InteractWithDraggedObject(GameObject draggedObject)
    {
        embedShape = draggedObject;
        draggedObject.transform.parent = transform;
        draggedObject.transform.position = Vector3.zero;
    }
    public void VerifyShape()
    {
        ShapeClassification embedShapeClass = embedShape.GetComponent<Shapes>().GetClassification();
        if(embedShapeClass == correctShape)
        {
            verificationIndicator.color = correct;
        }
        else
        {
            verificationIndicator.color = wrong;
        }
    }
    public void resetColorIndicator()
    {
        verificationIndicator.color = normal;
    }
}
