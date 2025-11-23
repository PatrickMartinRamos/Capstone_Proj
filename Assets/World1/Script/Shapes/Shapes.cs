using DG.Tweening;
using System.Collections.Generic;
using System.Net;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using Sequence = DG.Tweening.Sequence;
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
    internal bool isGiven = false, isMovable = true;

    protected GameObject initParent;

    public string ID => iD;

    protected virtual void Start()
    {
        // Generate ID
        iD = Generate(7);
        ChangeOriginPos(transform.localPosition);
        origScaleSize = transform.localScale.x;
        if (StageManager.Instance.StageNumber % 3 == 1) withValue = false;
        if(!withValue) valueLabel.SetActive(false);
        initParent = transform.parent.gameObject;
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
        if (StageManager.Instance.problemType == ProblemType.completingSquare)
        {
            GameObject newShape = Instantiate(StageManager.Instance.squaredConstant, target.transform.position, Quaternion.identity);
            newShape.GetComponent<Shapes>().AddValue(target.GetComponent<Shapes>().value, dragged.GetComponent<Shapes>().value);
            newShape.transform.SetParent(StageManager.Instance.craftArea.transform);
            Destroy(dragged);
            Destroy(target);
        }
        target.GetComponent<Shapes>().AddValue(dragged.GetComponent<Shapes>().value);
        Destroy(dragged);
    }
    // Function for combining shapes in different vessels
    public virtual void CombineShapes(GameObject dragged, GameObject target)
    {

    }
    public void AddValue (int val)
    {
        if (StageManager.Instance.problemType != ProblemType.squaringBinomial)
            value *= val;
        else
            value += val;
        valueLabel.GetComponent<TextMeshProUGUI>().text = value.ToString();

        if (!withValue)
        {
            withValue = true;
            valueLabel.SetActive(true);
        }
    }
    private List<int> valList;
    private int currentIndex = 0;
    public void AddValue(int val1, int val2)
    {

        if (StageManager.Instance.problem.stageDifficulty != Difficulty.hard || StageManager.Instance.problemType == ProblemType.completingSquare)
        {
            value = val1 * val2;
            withValue = true;
            valueLabel.GetComponent<TextMeshProUGUI>().text = value.ToString();

            if (StageManager.Instance.problem.stageDifficulty != Difficulty.easy)
                valueLabel.SetActive(true);
        }
        else
        {
            int[] valSet = new int[]
            {
            Random.Range(1, 6),
            val1 * val2,
            Random.Range(1, 9)
            };

            valList = new List<int>(valSet);
            ShuffleList(valList);

            currentIndex = 0;   // IMPORTANT FIX

            // Assign the first value to the shape immediately
            value = valList[currentIndex];
            currentIndex++;

            withValue = true;
            valueLabel.SetActive(true);
            valueLabel.GetComponent<TextMeshProUGUI>().text = value.ToString();
        }
    }

    public virtual void AddValue(Difficulty difficulty)
    {
        withValue = true;
    }
    public virtual void AddQuotientValue(int val)
    {
        value = val;
        withValue = true;
        valueLabel.GetComponent<TextMeshProUGUI>().text = value.ToString();
        valueLabel.SetActive(true);
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
    protected Vector3 GetRandomPosition(GameObject targetArea)
    {
        // Get collider bounds
        Bounds bounds = targetArea.GetComponent<Collider2D>().bounds;

        // Pick random position inside the bounds
        float randomX = Random.Range(bounds.min.x, bounds.max.x);
        float randomY = Random.Range(bounds.min.y, bounds.max.y);

        return new Vector3(randomX, randomY, 0);
    }
    public void FixScale()
    {
        transform.DOScale(origScaleSize, 0.5f);
    }
    public void FixScale(float scaleModifier)
    {
        transform.DOScale(origScaleSize * scaleModifier, 0.5f);
    }

    public virtual void MoveToArea(Transform pos = null)
    {
        if (pos != null)
        {
            transform.SetParent(pos);
            Debug.Log($"Moving to {pos}.");
        }
        else
            transform.SetParent(StageManager.Instance.craftArea.transform);

        // Apply impulse force once
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        Vector3 targetPos = GetRandomPosition(transform.parent.gameObject);
        rb.DOMove(targetPos, 1f).OnComplete(()=> 
        { 
            ChangeOriginPos(targetPos);
            GetComponentInParent<AreaContainer>().InteractWithDraggedObject(this.gameObject);
        });
        Debug.Log("Sending to Area");
        //StageManager.Instance.craftArea.GetComponent<CraftAreaMech>().InteractWithDraggedObject(this.gameObject);
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
    public void returnToInitParent()
    {
        transform.parent = initParent.transform;
        transform.localPosition = originPos;
        FixScale();
    }
    // Fisher–Yates shuffle
    private void ShuffleList(List<int> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int rand = Random.Range(0, i + 1);
            int temp = list[i];
            list[i] = list[rand];
            list[rand] = temp;
        }
    }

    // Call this function to get one value each time
    public int GetNextValue()
    {
        if (currentIndex >= valList.Count)
        {
            Debug.LogWarning("All values already used!");
            return -1;
        }

        int nextValue = valList[currentIndex];
        currentIndex++;             // move to next
        return nextValue;
    }

}

