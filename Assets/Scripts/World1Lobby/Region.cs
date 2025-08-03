using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Region : MonoBehaviour
{
    [SerializeField] private RegionStatusScriptable RegionStatus;
    [SerializeField] private TextMeshProUGUI stageNumber;
    //[SerializeField] private GameObject stageLock;
    [SerializeField] private GameObject playBtn;

    [SerializeField] private Vector3 targetEulerAngle;
    private Vector3 currentEulerAngle;
    [SerializeField] private float rotateTime, time;

    private bool isWorldRotating = false;
    private GameObject World;

    private void Start()
    {
        playBtn.GetComponent<Button>().onClick.AddListener(PlayStage);
    }
    private void Update()
    {
        if (RegionStatus.isSelected && isWorldRotating)
        {
            StartCoroutine(RotateOverTime());
            if(World.transform.eulerAngles == targetEulerAngle)
            {
                StopCoroutine(RotateOverTime());
                isWorldRotating = false;
            }
        }
    }
    void PlayStage()
    {
        if (RegionStatus.isSelected)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
    public void UnselectRegion()
    {
        RegionStatus.isSelected = false;
    }
    public void RotateWorld(GameObject world)
    {
        RegionStatus.isSelected = true;
        stageNumber.text = RegionStatus.stageNumber.ToString();
        World = world;
        currentEulerAngle = World.transform.eulerAngles;
        isWorldRotating = true;
    }
    IEnumerator RotateOverTime()
    {
        Vector3 startEuler = World.transform.eulerAngles;
        float time = 0f;

        while (time < rotateTime)
        {
            time += Time.deltaTime;
            float t = time / rotateTime;
            World.transform.eulerAngles = Vector3.Lerp(startEuler, targetEulerAngle, t);
            yield return null;
        }

        // Ensure final angle is set
        World.transform.eulerAngles = targetEulerAngle;
    }
}
