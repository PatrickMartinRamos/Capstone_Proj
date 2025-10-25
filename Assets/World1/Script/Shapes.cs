using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
public class ShapeStats
{
    public string iD;
    public ShapeClassification classification;
    public string name;
    public bool withValue;
    public float value;

}
public enum ShapeClassification
{
    Square,
    Circle,
    Triangle,
    Star,
    Scissors
}
public class Shapes : MonoBehaviour, IDraggable, Itargetable
{
    [SerializeField] string iD;
    [SerializeField] ShapeClassification classification;
    [SerializeField]
    private string shapeName;
    public string Name => shapeName;
    [SerializeField] bool withValue;
    [SerializeField] float value;
    [SerializeField] Vector3 originPos;
    [SerializeField] List<string> interacted;

    public string ID => iD;

    void Start()
    {
        // Generate ID
        iD = Generate(7);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool Interact(GameObject target, Vector3 spawnPt)
    {
        // Get the info of target shape
        ShapeStats targetShape = new ShapeStats();
        targetShape = target.GetComponent<Itargetable>().SendInfo();

        // Check if already interacteed with
        for (int i = 0; i < interacted.Count; i++)
        {
            if (targetShape.iD == interacted[i])
            {
                StageManager.Instance.NotificationText.text = "Please try a new Combination.";
            }
            return false;
        }
        interacted.Add(targetShape.iD);
        switch (classification)
        {
            case ShapeClassification.Square:
                Debug.Log("Matching Square...");
                return shapeCombination.Instance.Square(targetShape.classification, spawnPt);
                
            case ShapeClassification.Circle:
                Debug.Log("Matching Circle...");
                return shapeCombination.Instance.Circle(targetShape.classification, spawnPt);
                
            case ShapeClassification.Triangle:
                Debug.Log("Matching Triangle...");
                return shapeCombination.Instance.Triangle(gameObject, target, spawnPt);
                
            case ShapeClassification.Scissors:
                Debug.Log("Dividing...");
                return false;
                
            default:
                Debug.Log("Matched unknown shape");
                return false;
                
        }
    }
    public ShapeClassification GetDraggedObjectClassification()
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
    public ShapeClassification GetTargetClassification()
    {
        return classification;
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
