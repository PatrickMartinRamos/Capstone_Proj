using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

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
                if(spawnedShapesList.IndexOf(shape) < symbols.Count) symbols[spawnedShapesList.IndexOf(shape)].SetActive(true);
            }
        }
        if (spawnedShapesList.Count == 4)
        {
            bool areEqual = spawnedShapesArrangement.SequenceEqual(secondArrangement);
            if (areEqual)
            {
                Debug.Log("Step 2 Squaring Binomial: Check");
            }
            else
            {
                foreach (GameObject shape in spawnedShapesList)
                {
                    Debug.Log("Wrong Combination");
                }
                foreach (var symbol in symbols)
                {
                    symbol.SetActive(true);
                }
                //spawnedShapesList.Clear();
            }
        }
    }
    public bool Square(string matchedTag, Vector3 spawnPos)
    {
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
    public bool Circle(string matchedTag, Vector3 spawnPos)
    {
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
    public bool Triangle(string matchedTag, Vector3 spawnPos)
    {
        return false;
    }
    void SpawnShape (GameObject shape, Vector3 spawnPos)
    {
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
