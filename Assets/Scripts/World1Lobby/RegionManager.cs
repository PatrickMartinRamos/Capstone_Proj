using System.Collections.Generic;
using UnityEngine;
using DG.Tweening; 

public class RegionManager : MonoBehaviour
{
    [SerializeField] private GameObject World;
    [SerializeField] private List<Transform> regions;

    private int selectedRegionIndex = 0;
    private GameObject selectedRegion;
    GameObject SelectedRegion => selectedRegion;

    private void Start()
    {
        selectedRegion = regions[selectedRegionIndex].gameObject;
        selectedRegion.GetComponent<Region>().InitiateStageLabelChange();
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
        selectedRegion = regions[selectedRegionIndex].gameObject;
        selectedRegion.GetComponent<Region>().RotateWorld(World);
    }

}
