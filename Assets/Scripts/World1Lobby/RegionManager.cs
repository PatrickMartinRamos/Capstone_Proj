using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RegionManager : MonoBehaviour
{
    public static RegionManager instance;
    [SerializeField] private GameObject currentWorldSelected;
    [SerializeField] private List<GameObject> regions = new List<GameObject>();
    [SerializeField] private int selectedRegionIndex = 0;
    [SerializeField] private GameObject tintBG;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        tintBG.gameObject.SetActive(false);
    }

    public void InitializedWorldStage(Transform world)
    {
        ClearWorldStages();

        currentWorldSelected = world.gameObject;
        tintBG.gameObject.SetActive(true);
        // Find all Region scripts inside the world object
        Region[] foundRegions = world.GetComponentsInChildren<Region>(true);

        int index = 0;
        foreach (Region region in foundRegions)
        {
            region.InitiateStageLabelChange();
            GameObject regionObj = region.gameObject;
            regions.Add(regionObj);

            //Debug.Log($"Region Found: {regionObj.name}");

            // Try to get button and add listener
            Button btn = regionObj.GetComponent<Button>();
            if (btn != null)
            {
                int capturedIndex = index; // local copy for lambda
                btn.onClick.RemoveAllListeners(); // clear previous listeners
                btn.onClick.AddListener(() => OnRegionClicked(capturedIndex, regionObj.name));
            }

            index++;
        }

        // Debug.Log($"Current World: {currentWorldSelected.name}");
        // Debug.Log($"Total Regions Found: {regions.Count}");
    }

    private void OnRegionClicked(int index, string regionName)
    {
        selectedRegionIndex = index;
        // Debug.Log($"Clicked Region: {regionName} (Index: {index})");

        // Load scene based on RegionStatus
        Region region = regions[index].GetComponent<Region>();
        if (region != null)
        {
            string sceneName = region.RegionStatus.worldName;
            // Debug.Log($"Loading Scene: {sceneName}");
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
        }
    }

    public void ClearWorldStages()
    {
        // Clear existing listeners to avoid duplicates
        foreach (var regionObj in regions)
        {
            Button btn = regionObj.GetComponent<Button>();
            if (btn != null) btn.onClick.RemoveAllListeners();
        }

        regions.Clear();
        tintBG.gameObject.SetActive(false);
        currentWorldSelected = null;
    }

    public void LoadSelectedStage()
    {
        Debug.Log($"Loading stage for region index: {selectedRegionIndex}");
    }
}
