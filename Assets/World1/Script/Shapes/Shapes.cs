using System.Collections.Generic;
using UnityEngine;
public class ShapeStats
{
    public string iD;
    public ShapeClassification classification;
    public string name;
    public bool withValue;
    public float value;

}

public class Shapes : MonoBehaviour, ITargetable
{
    [SerializeField] string iD;
    [SerializeField] ShapeClassification classification;
    [SerializeField]
    private string shapeName;
    public string Name => shapeName;
    [SerializeField] internal bool withValue;
    [SerializeField] internal int value;
    [SerializeField] internal Vector3 originPos;
    [SerializeField] internal List<string> interacted;
    internal Vector3 spawnPt;

    public string ID => iD;

    void Start()
    {
        // Generate ID
        iD = Generate(7);
        ChangeOriginPos(transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Function as ITargetable
    public void InteractWithDraggedObject(GameObject draggedObject)
    {
        draggedObject.GetComponent<Shapes>().Interact(this.gameObject);
    }

    public virtual void Interact(GameObject target)
    {
        Debug.Log(target.name + " \n" +  this.gameObject.name);
        // Get the info of target shape
        ShapeStats targetShape = new ShapeStats();
        targetShape = target.GetComponent<Shapes>().SendInfo();

        // Check if in the same parent
        if (target.gameObject.transform.parent.gameObject == this.transform.parent.gameObject)
        {
            StageManager.Instance.NotificationText.text = "Cannot COMBINE a VARIABLE and a CONSTANT NUMBER.";
            RevertPosition();
            return;
        }


/*        // Check if already interacteed with
        for (int i = 0; i < interacted.Count; i++)
        {
            if (targetShape.iD == interacted[i])
            {
                StageManager.Instance.NotificationText.text = "Please try a new Combination.";
                return;
            }
        }*/

        // ADD if not yet interacted with
        interacted.Add(targetShape.iD);
        target.GetComponent<Shapes>().AddInteractedShape(iD);
        CombineShapes(this.gameObject, target);
        RevertPosition();
    }
    public virtual void CombineShapes(GameObject dragged, GameObject target)
    {

    }
    public virtual void AddValue(Difficulty difficulty)
    {
        withValue = true;
    }
    public void AddInteractedShape(string iD)
    {
        interacted.Add(iD);
    }
    public ShapeClassification GetClassification()
    {
        return classification;
    }
    
    public ShapeStats SendInfo()
    {
        ShapeStats shape = new ShapeStats();
        shape.iD = iD;
        shape.classification = classification;
        shape.withValue = withValue;
        shape.value = value;
        return shape;
    }

    public void RevertPosition()
    {
        this.transform.position = originPos;
    }
    public void ChangeOriginPos(Vector3 pos)
    {
        originPos = pos;
    }

    // Random ID Generator - AlphaNumeric
    private const string Chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

    public static string Generate(int length)
    {
        if (length <= 0) return string.Empty;

        System.Text.StringBuilder sb = new System.Text.StringBuilder(length);
        for (int i = 0; i < length; i++)
            sb.Append(Chars[Random.Range(0, Chars.Length)]);

        return sb.ToString();
    }

}
