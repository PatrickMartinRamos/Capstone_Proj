using UnityEngine;

public class guidePanelClickPrevention : MonoBehaviour
{
    [SerializeField] TutorialPromptBox tutorialBox;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnEnable()
    {
        tutorialBox.turnOffOnPointDown(true);
    }
    private void OnDisable()
    {
        tutorialBox.turnOffOnPointDown(false);

    }
}
