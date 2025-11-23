using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CloseButton : ButtonEvent
{
    [SerializeField] private List<GameObject> objectToClose = new();
    [SerializeField] bool isSceneChanger = false;
    [SerializeField] bool isStageChanger = false;

    [ShowIf("isSceneChanger")] [SerializeField] string sceneName;

    internal override void OnClick()
    {
        base.OnClick();
        if (isSceneChanger)
        {
            StartCoroutine(WaitForAudioToFinish());
        }
        else foreach (GameObject obj in objectToClose)
        {
            obj.SetActive(false);
        }
    }
    void ChangeScene()
    {
        if (isStageChanger)
        {
            int currentStage = PlayerPrefs.GetInt("StageID");
            PlayerPrefs.SetInt("StageID", currentStage+1);
        }
        SceneManager.LoadScene(sceneName);
    }

    private bool fiftyPercentTriggered = false;

    IEnumerator WaitForAudioToFinish()
    {
        // Wait until the audio starts playing
        yield return new WaitUntil(() => audioSource.isPlaying);

        // Calculate the 50% mark
        float fiftyPercentTime = audioSource.clip.length * 0.5f;

        while (audioSource.isPlaying)
        {
            if (!fiftyPercentTriggered && audioSource.time >= fiftyPercentTime)
            {
                foreach (GameObject obj in objectToClose)
                {
                    obj.SetActive(false);
                }
            
                ChangeScene();
                fiftyPercentTriggered = true; // Prevent multiple calls
            }
            yield return null; // Wait for the next frame
        }

        // Reset for potential future plays (e.g., if the audio loops or is played again)
        fiftyPercentTriggered = false;
    }
}
