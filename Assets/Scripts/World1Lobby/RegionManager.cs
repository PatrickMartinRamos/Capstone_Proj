using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class RegionManager : MonoBehaviour
{
    [SerializeField] private GameObject World;
    [SerializeField] private List<GameObject> regions;

    private int selectedRegionIndex = 0;
    private GameObject selectedRegion;
    GameObject SelectedRegion => selectedRegion;

    private void Start()
    {
        selectedRegion = regions[selectedRegionIndex];
        World.transform.Rotate(0, 0, 0);  
    }

    public void ToNextStage()
    {
        Debug.Log("Next");
        selectedRegionIndex = Mathf.Clamp(selectedRegionIndex+1,0,3);
        ChangeStage();
    }
    public void ToPreviousStage()
    {
        Debug.Log("Previous");
        selectedRegionIndex = Mathf.Clamp(selectedRegionIndex-1, 0, 3);
        ChangeStage();
    } 
    private void ChangeStage()
    {
        selectedRegion.GetComponent<Region>().UnselectRegion();
        selectedRegion = regions[selectedRegionIndex];
        selectedRegion.GetComponent<Region>().RotateWorld(World);
    }

}
