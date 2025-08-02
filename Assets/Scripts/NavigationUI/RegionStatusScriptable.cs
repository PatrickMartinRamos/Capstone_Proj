using UnityEngine;

[CreateAssetMenu(fileName = "RegionStatus", menuName = "Region Status")]
public class RegionStatusScriptable : ScriptableObject
{
    public int stageNumber;
    public bool isUnlocked = false, isSelected = false;
    [Range(0, 3)] public float clearLevel;
}