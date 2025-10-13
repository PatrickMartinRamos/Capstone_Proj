using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RegionManager : MonoBehaviour
{

/// <summary>
/// TODO:
///     -update stage 
///     -check if what world is selected
///     -change the unlocked stage base on what world is selected
///     -load stage
///     -stage selection UI logic
/// </summary>

    [SerializeField] private GameObject world;
    [SerializeField] private List<Transform> regions;

    private int selectedRegionIndex = -1;
    private GameObject selectedRegion;

    private void Start()
    {
        // Attach click listeners for each region button
        for (int i = 0; i < regions.Count; i++)
        {
            int index = i;
            var regionBtn = regions[i].GetComponent<Button>();
            // Initialize stage label
            regions[i].GetComponent<Region>().InitiateStageLabelChange();
            regionBtn.onClick.AddListener(() => OnRegionClicked(index));
        }
    }

    void OnRegionClicked(int index)
    {
        if (selectedRegion != null)
        {
            selectedRegion.GetComponent<Region>().UnselectRegion();
        }

        // Update selected region info
        selectedRegionIndex = index;
        selectedRegion = regions[index].gameObject;

        // Mark as selected in Region script
        selectedRegion.GetComponent<Region>().SelectWorld(world);

        // Log selected region details
        var regionData = selectedRegion.GetComponent<Region>();
        Debug.Log($"Selected Region: {selectedRegion.name} | Stage: {regionData.name}");
    }
}
