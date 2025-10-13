using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Region : MonoBehaviour
{
    [SerializeField] private RegionStatusScriptable RegionStatus;
    [SerializeField] private TextMeshProUGUI stageNumber;
    [SerializeField] private GameObject playBtn;


    private bool isWorldRotating = false;
    private GameObject World;

    private void Start()
    {
        playBtn.GetComponent<Button>().onClick.AddListener(PlayStage);
    }
    public void InitiateStageLabelChange()
    {
        RegionStatus.isSelected = true;
        stageNumber.text = RegionStatus.stageNumber.ToString();
    }
    void PlayStage()
    {
        playBtn.GetComponent<Button>().onClick.RemoveAllListeners();

        if (RegionStatus.isSelected)
        {
            SceneManager.LoadScene(RegionStatus.worldName);
        }
    }
    public void UnselectRegion()
    {
        RegionStatus.isSelected = false;
    }
    public void SelectWorld(GameObject world)
    {
        RegionStatus.isSelected = true;
        stageNumber.text = RegionStatus.stageNumber.ToString();
        World = world;
        isWorldRotating = true;
    }
}
