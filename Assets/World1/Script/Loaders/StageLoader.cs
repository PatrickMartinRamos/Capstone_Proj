using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class World1StageLoader : MonoBehaviour
{
    [SerializeField] private int stageLevel;
    [SerializeField] NavBarManager navBarManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (stageLevel == 0)stageLevel = PlayerPrefs.GetInt("StageID");
        navBarManager.ChangeLevelIndicator(stageLevel);
        SceneManager.LoadScene("World1_Stage" + stageLevel.ToString(), LoadSceneMode.Additive);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
