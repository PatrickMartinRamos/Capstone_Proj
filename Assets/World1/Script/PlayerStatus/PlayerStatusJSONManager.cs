using System.IO;
using UnityEngine;
using static UnityEngine.JsonUtility;

public class PlayerStatusJSONManager : MonoBehaviour
{
    [SerializeField] private AlgebraicFoundationPlayerStatus algebWorldPlayerStatus;
    [SerializeField] private RegionStatusScriptable stage1;
    [SerializeField] private RegionStatusScriptable stage2;
    [SerializeField] private RegionStatusScriptable stage3;
    [SerializeField] private RegionStatusScriptable stage4;
    [SerializeField] private RegionStatusScriptable stage5;
    [SerializeField] private RegionStatusScriptable stage6;
    [SerializeField] private RegionStatusScriptable stage7;
    [SerializeField] private RegionStatusScriptable stage8;
    [SerializeField] private RegionStatusScriptable stage9;
    [SerializeField] private RegionStatusScriptable stage10;
    [SerializeField] private RegionStatusScriptable stage11;
    [SerializeField] private RegionStatusScriptable stage12;
    [SerializeField] private RegionStatusScriptable stage13;
    [SerializeField] private RegionStatusScriptable stage14;
    [SerializeField] private RegionStatusScriptable stage15;
    public void LoadPlayerStatus()
    {
        if (File.Exists(Application.dataPath + "/Data/data.json"))
        {
            string playerData = File.ReadAllText(Application.dataPath + "/Data/data.json");
            FromJsonOverwrite(playerData, algebWorldPlayerStatus);

            stage1.clearLevel = algebWorldPlayerStatus.stage1ClearLevel;
            stage2.clearLevel = algebWorldPlayerStatus.stage2ClearLevel;
            stage3.clearLevel = algebWorldPlayerStatus.stage3ClearLevel;
            stage4.clearLevel = algebWorldPlayerStatus.stage4ClearLevel;
            stage5.clearLevel = algebWorldPlayerStatus.stage5ClearLevel;
            stage6.clearLevel = algebWorldPlayerStatus.stage6ClearLevel;
            stage7.clearLevel = algebWorldPlayerStatus.stage7ClearLevel;
            stage8.clearLevel = algebWorldPlayerStatus.stage8ClearLevel;
            stage9.clearLevel = algebWorldPlayerStatus.stage9ClearLevel;
            stage10.clearLevel = algebWorldPlayerStatus.stage10ClearLevel;
            stage11.clearLevel = algebWorldPlayerStatus.stage11ClearLevel;
            stage12.clearLevel = algebWorldPlayerStatus.stage12ClearLevel;
            stage13.clearLevel = algebWorldPlayerStatus.stage13ClearLevel;
            stage14.clearLevel = algebWorldPlayerStatus.stage14ClearLevel;
            stage15.clearLevel = algebWorldPlayerStatus.stage15ClearLevel;
        }

        else 
        {
            UpdatePlayerStatus();
        }
    }
    public void UpdatePlayerStatus()
    {
        algebWorldPlayerStatus.stage1ClearLevel = stage1.clearLevel;
        algebWorldPlayerStatus.stage2ClearLevel = stage2.clearLevel;
        algebWorldPlayerStatus.stage3ClearLevel = stage3.clearLevel;
        algebWorldPlayerStatus.stage4ClearLevel = stage4.clearLevel;
        algebWorldPlayerStatus.stage5ClearLevel = stage5.clearLevel;
        algebWorldPlayerStatus.stage6ClearLevel = stage6.clearLevel;
        algebWorldPlayerStatus.stage7ClearLevel = stage7.clearLevel;
        algebWorldPlayerStatus.stage8ClearLevel = stage8.clearLevel;
        algebWorldPlayerStatus.stage9ClearLevel = stage9.clearLevel;
        algebWorldPlayerStatus.stage10ClearLevel = stage10.clearLevel;
        algebWorldPlayerStatus.stage11ClearLevel = stage11.clearLevel;
        algebWorldPlayerStatus.stage12ClearLevel = stage12.clearLevel;
        algebWorldPlayerStatus.stage13ClearLevel = stage13.clearLevel;
        algebWorldPlayerStatus.stage14ClearLevel = stage14.clearLevel;
        algebWorldPlayerStatus.stage15ClearLevel = stage15.clearLevel;

        string playerData = ToJson(algebWorldPlayerStatus);
        File.WriteAllText(Application.dataPath + "/Data/data.json", playerData);
    }
}
