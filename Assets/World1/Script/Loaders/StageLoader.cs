using UnityEngine;
using UnityEngine.SceneManagement;

public class StageLoader : MonoBehaviour
{
    [SerializeField] private int stageLevel;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (stageLevel == 0)stageLevel = PlayerPrefs.GetInt("StageID");
        SceneManager.LoadScene("World1_Stage" + stageLevel.ToString(), LoadSceneMode.Additive);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
