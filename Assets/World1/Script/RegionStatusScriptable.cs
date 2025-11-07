using UnityEngine;

[CreateAssetMenu(fileName = "RegionStatus", menuName = "Region Status")]
public class RegionStatusScriptable : ScriptableObject
{
    public string worldName;
    public int stageNumber;
    public bool isUnlocked = false, isSelected = false;
    [Range(0, 3)] public int clearLevel;
}