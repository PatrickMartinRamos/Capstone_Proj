using DG.Tweening;
using System.Collections.Generic;
using System.Net;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
public class ShapeStats
{
    public string iD;
    public ShapeClassification classification;
    public string name;
    public bool withValue;
    public int value;

}

public class Shapes : MonoBehaviour, ITargetable
{
    [SerializeField] string iD;
    [SerializeField] ShapeClassification classification;
    [SerializeField]
    private string shapeName;
    public string Name => shapeName;
    [SerializeField] internal bool withValue = false;
    [SerializeField] internal int value;
    [SerializeField] internal Vector3 originPos;
    [SerializeField] internal List<string> interacted;
    internal Vector3 spawnPt;
    [SerializeField] float origScaleSize;
    [SerializeField] internal GameObject valueLabel;
    internal bool isGiven = false;

    public string ID => iD;

    void Start()
    {
        // Generate ID
        iD = Generate(7);
        ChangeOriginPos(transform.localPosition);
        origScaleSize = transform.localScale.x;
        if (StageManager.Instance.StageNumber % 3 == 1) withValue = false;
        if(!withValue) valueLabel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ChangeToGiven()
    {
        isGiven = true;
    }

    // Function as ITargetable
    public void InteractWithDraggedObject(GameObject draggedObject)
    {
        draggedObject.GetComponent<Shapes>().Interact(this.gameObject);
    }

    public virtual void Interact(GameObject target)
    {
        // Get the info of target shape

        ShapeStats targetShape = new ShapeStats();
        targetShape = target.GetComponent<Shapes>().SendInfo();

        interacted.Add(targetShape.iD);
        target.GetComponent<Shapes>().AddInteractedShape(iD);

        // Check if in the same parent
        if (target.gameObject.transform.parent.gameObject == this.transform.parent.gameObject)
            CombineLikeTerms(this.gameObject, target);
        else CombineShapes(this.gameObject, target);

        RevertPosition();
    }
    // Function for combining shapes in similar vessel
    public virtual void CombineLikeTerms(GameObject dragged, GameObject target)
    {
        if (target.GetComponent<Shapes>().classification != dragged.GetComponent<Shapes>().classification) return; 
        target.GetComponent<Shapes>().AddValue(dragged.GetComponent<Shapes>().value);
        Destroy(dragged);
    }
    // Function for combining shapes in different vessels
    public virtual void CombineShapes(GameObject dragged, GameObject target)
    {

    }
    public void AddValue (int val)
    {
        value += val;
        valueLabel.GetComponent<TextMeshProUGUI>().text = value.ToString();

        if (!withValue)
        {
            withValue = true;
            valueLabel.SetActive(true);
        }
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
        this.transform.localPosition = originPos;
    }
    public void ChangeOriginPos(Vector3 pos)
    {
        originPos = pos;
    }

    // getting spot in assigned area for combination insatantiation
    private Vector3 GetRandomPosition()
    {
        // Get collider bounds
        Bounds bounds = StageManager.Instance.craftArea.GetComponent<Collider2D>().bounds;

        // Pick random position inside the bounds
        float randomX = Random.Range(bounds.min.x, bounds.max.x);
        float randomY = Random.Range(bounds.min.y, bounds.max.y);

        return new Vector3(randomX, randomY, 0);
    }
    public void FixScale()
    {
        transform.DOScale(origScaleSize, 0.5f);
    }

    public void MoveToArea()
    {
        transform.SetParent(StageManager.Instance.craftArea.transform);

        // Apply impulse force once
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        Vector3 targetPos = GetRandomPosition();
        rb.DOMove(targetPos, 1f);
        Debug.Log("Sending to Area");
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
