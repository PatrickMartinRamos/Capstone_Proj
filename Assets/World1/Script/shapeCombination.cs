using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.Rendering.ProbeAdjustmentVolume;

public class shapeCombination : MonoBehaviour
{
    [Header("Spawnable Shapes")]
    [Header("Variable:")]
    [SerializeField] private GameObject variable;

    [Header("Multipled Numbers:")]
    [SerializeField] private GameObject squaredVariable;
    [SerializeField] private GameObject product;
    [SerializeField] private GameObject squaredConstant;

    [Header("Numbers:")]
    [SerializeField] private GameObject prime;
    [SerializeField] private GameObject composite;
    [SerializeField] private GameObject perfectSquare;
    [SerializeField] private GameObject constant;

    [Header("Stars")]
    [SerializeField] private GameObject star_4pt;
    [SerializeField] private GameObject star_5pt;
    [SerializeField] private GameObject star_6pt;

    [Header("Effects")]
    [Header("Visual")]
    [SerializeField] private GameObject vfxPrefab;
    [Header("Audio")]
    [SerializeField] private AudioClip matchSFX;
    public AudioSource audioSource;

    [Header("Spawned Positions")]
    [SerializeField] List<Transform> spawnPts;

    [Header("Arrangement Checker")]
    [SerializeField] string[] secondArrangement = { "Squared Variable", "Product", "Product", "Squared Constant" };
    [SerializeField] string[] spawnedShapesArrangement;
    [SerializeField] private List<GameObject> symbols = new List<GameObject>();
    private GameObject spawnedShape;
    private List<GameObject> spawnedShapesList = new List<GameObject>();
    private bool areEqual = false, correctCombination = false, doneMoving = false;
    private Vector3 FinalPos;


    public static shapeCombination Instance {  get; private set; }


    //[SerializeField] private Sprite triangle;
    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        spawnedShapesArrangement = new string[]{" ", " ", " ", " " };
    }
    private void Update()
    {
        if (spawnedShapesList.Count != 0)
        {
            foreach(var shape in spawnedShapesList)
            {
                if (shape.transform.position != spawnPts[spawnedShapesList.IndexOf(shape)].position)
                {
                    Vector2 currentPos = shape.transform.position;
                    Vector2 targetPos = spawnPts[spawnedShapesList.IndexOf(shape)].position;
                    shape.transform.position = Vector2.MoveTowards(currentPos, targetPos, 2*Time.deltaTime);
                }
                if (spawnedShapesList.IndexOf(shape) == 3 && shape.transform.position == spawnPts[spawnedShapesList.IndexOf(shape)].position) 
                    areEqual = true;
            }
            if (!areEqual)
            {
                foreach (var symbol in symbols)
                    symbol.SetActive(true);
            }
        }
        if (spawnedShapesList.Count == 4)
        {
            areEqual = spawnedShapesArrangement.SequenceEqual(secondArrangement);
            if (areEqual)
            {
                Debug.Log("Step 2 Squaring Binomial: Check");
            }
            else
            {
                StageManager.Instance.OpenWrongAnswerPanel();
            }
        }
        if (correctCombination)
        {
            var curPos1 = spawnedShapesList[0].transform.position;
            spawnedShapesList[0].transform.position = Vector3.MoveTowards(curPos1, FinalPos, 5 * Time.deltaTime);
            var curPos2 = spawnedShapesList[3].transform.position;
            spawnedShapesList[3].transform.position = Vector3.MoveTowards(curPos1, FinalPos, 5 * Time.deltaTime);
            if (spawnedShapesList[0].transform.position == FinalPos &&
                spawnedShapesList[3].transform.position == FinalPos)
            {
                StageManager.Instance.Star = Instantiate(star_4pt, FinalPos, Quaternion.identity);
                ClearShapeList();
                StageManager.Instance.ShowCraftBox = false;
            }

        }
    }
    public void ClearShapeList()
    {
        foreach (GameObject shape in spawnedShapesList)
        {
            Destroy(shape);
        }
        spawnedShapesList.Clear();
        foreach (var symbol in symbols)
        {
            symbol.SetActive(false);
        }
    }
    public bool Square(GameObject targetObject, Vector3 spawnPos)
    {
        string matchedTag = targetObject.gameObject.tag;
        switch (matchedTag)
        {
            case "Square":
                SpawnShape(squaredVariable, spawnPos);
                break;
            case "Circle":
                PlayFX(spawnPos);
                SpawnShape(product, spawnPos);
                break;
            default:
                Debug.Log("No Match");
                return false;
        }
        return true;
    }
    public bool Circle(GameObject targetObject, Vector3 spawnPos)
    {
        string matchedTag = targetObject.gameObject.tag;
        switch (matchedTag)
        {
            case "Square":
                SpawnShape(product, spawnPos);
                break;
            case "Circle":
                PlayFX(spawnPos);
                SpawnShape(squaredConstant, spawnPos);
                break;
            default:
                Debug.Log("No Match");
                return false;
        }
        return true;
    }
    public bool Triangle(GameObject draggedObject, GameObject targetObject, Vector3 spawnPos)
    {
        string matchedTag = targetObject.gameObject.tag;
        switch (matchedTag)
        {
            case "Triangle":
                if (targetObject.GetComponent<DraggableShape>().Name == "Product" &&
                    draggedObject.GetComponent<DraggableShape>().Name == "Product")
                {
                    Vector3 newPosition = (targetObject.transform.position + draggedObject.transform.position) / 2;

                    targetObject.transform.position = newPosition;
                    targetObject.transform.localScale = targetObject.transform.localScale * 2;
                    targetObject.GetComponent<DraggableShape>().ChangeOriginPos(new Vector3 (0,0,0));
                    symbols[1].gameObject.SetActive(false);
                    draggedObject.SetActive(false);
                    correctCombination = true;
                    FinalPos = targetObject.transform.position;
                    return true;
                }
                else
                {
                    StageManager.Instance.DeductTime(1f);
                    StageManager.Instance.OpenWrongAnswerPanel();
                    return false;
                }
                break;
            default:
                Debug.Log("No Match");
                return false;
        }
        return true;

    }
    void SpawnShape (GameObject shape, Vector3 spawnPos)
    {
        if (spawnedShapesList.Count == 4) return;
        PlayFX(spawnPos);
        spawnedShape = Instantiate(shape, spawnPos, Quaternion.identity);
        spawnedShapesArrangement[spawnedShapesList.Count] = spawnedShape.GetComponent<DraggableShape>().Name;
        spawnedShapesList.Add(spawnedShape);
    }
    void PlayFX(Vector3 spawnPos)
    {
        //Play VFX
        if (vfxPrefab != null)
        {
            GameObject vfx = Instantiate(vfxPrefab, spawnPos, Quaternion.identity);
            Destroy(vfx, 2f); 
        }

        //Play SFX
        if (matchSFX != null && audioSource != null)
        {
            audioSource.PlayOneShot(matchSFX);
        }
    }
}
