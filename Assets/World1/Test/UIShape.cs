using UnityEngine;
using TMPro;
using DG.Tweening;
using System.Collections.Generic;

public class UIShape : MonoBehaviour, ITargetable
{
    [Header("Shape Data")]
    [SerializeField] [ReadOnly] string iD;
    [SerializeField] ShapeClassification classification;
    [SerializeField] protected string shapeName;
    public string Name => shapeName;

    [SerializeField] internal bool withValue = false;
    [SerializeField] internal int value;
    [SerializeField] internal List<string> interacted;
    [SerializeField] [ReadOnly] bool isGiven = false;

    [Header("UI References")]
    [SerializeField] internal GameObject valueLabel;

    [Header("Positioning")]
    [SerializeField] internal Vector2 originPos;   // anchoredPosition
    internal Vector2 spawnPos;
    float origScaleSize;

    RectTransform rect;
    Canvas canvas;

    public string ID => iD;

    void Awake()
    {
        rect = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
    }

    internal virtual void Start()
    {
        iD = Generate(7);
        ChangeOriginPos(rect.anchoredPosition);

        origScaleSize = rect.localScale.x;
    }

    // Transfers dragged info to the target
    public void InteractWithDraggedObject(GameObject draggedObject)
    {
        Debug.Log("Combining Shapes...");

        draggedObject.GetComponent<UIShape>().Interact(this.gameObject);
    }

    public virtual void Interact(GameObject dragged)
    {
        Debug.Log("Interacting...");

        UIShape draggedShapeComp = dragged.GetComponent<UIShape>();
        ShapeStats draggedShape = draggedShapeComp.SendInfo();
        Debug.Log(draggedShape.iD);

        interacted.Add(draggedShape.iD);
        draggedShapeComp.AddInteractedShape(iD);

        // Same parent = combine like terms
        if (dragged.transform.parent == transform.parent)
            CombineSimilarParent(this.gameObject, dragged);
        else
            CombineShapes(dragged, this.gameObject);

        RevertPosition();
    }

    // Combine inside same parent
    public virtual void CombineSimilarParent(GameObject dragged, GameObject target)
    {
        Debug.Log("Combining Like Terms...");

        UIShape d = dragged.GetComponent<UIShape>();
        UIShape t = target.GetComponent<UIShape>();

        if (d.classification != t.classification)
            return;

        t.AddValue(d.value);
        //Destroy(dragged);
    }

    // Combine across different containers
    public virtual void CombineShapes(GameObject dragged, GameObject target)
    {

        // Implement if needed
    }

    public void AddValue(int val)
    {
        value += val;

        // Update UI
        valueLabel.GetComponent<TextMeshProUGUI>().text = value.ToString();

        if (!withValue)
        {
            withValue = true;
            valueLabel.SetActive(true);
        }
    }

    public void AddValue(int val1, int val2)
    {
        value = val1 * val2;
        withValue = true;

        valueLabel.GetComponent<TextMeshProUGUI>().text = value.ToString();

        if (StageManager.Instance.problem.stageDifficulty != Difficulty.easy)
            valueLabel.SetActive(true);
    }

    public virtual void AddValue(Difficulty difficulty)
    {
        withValue = true;
    }

    public void AddInteractedShape(string id)
    {
        interacted.Add(id);
    }

    public ShapeClassification GetClassification() => classification;

    public ShapeStats SendInfo()
    {
        return new ShapeStats()
        {
            iD = iD,
            classification = classification,
            value = value,
            withValue = withValue
        };
    }

    public void RevertPosition()
    {
        rect.DOAnchorPos(originPos, 0.25f);
    }

    public void ChangeOriginPos(Vector2 pos)
    {
        originPos = pos;
    }

    public void FixScale()
    {
        rect.DOScale(origScaleSize, 0.5f);
    }

    public void FixScale(float scaleModifier)
    {
        rect.DOScale(origScaleSize * scaleModifier, 0.5f);
    }

    // UI Version – choose a random point WITHIN a UI RectTransform
    private Vector2 GetRandomPosition()
    {
        RectTransform craftRect = StageManager.Instance.craftArea.GetComponent<RectTransform>();

        float x = Random.Range(craftRect.rect.min.x, craftRect.rect.max.x);
        float y = Random.Range(craftRect.rect.min.y, craftRect.rect.max.y);

        return new Vector2(x, y);
    }

    // UI Version – move using anchoredPosition, no rigidbody
    public void MoveToArea()
    {
        transform.SetParent(StageManager.Instance.craftArea.transform);

        Vector2 target = GetRandomPosition();
        rect.DOAnchorPos(target, 1f).OnComplete(() => ChangeOriginPos(target));

        Debug.Log("Sending to Area");
    }

    // ID Generator
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
