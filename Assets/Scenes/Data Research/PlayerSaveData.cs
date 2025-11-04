[System.Serializable]
public class PlayerSaveData
{
    public string playerName;

    //Check stage level and stage score
    public int stageLevel;
    public int stageScore; // per star basis
    //---------------------------------

    public float playTime; // in seconds
}
