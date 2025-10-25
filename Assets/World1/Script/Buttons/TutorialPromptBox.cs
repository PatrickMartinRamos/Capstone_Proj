using UnityEngine;
using UnityEngine.UI;

public class TutorialPromptBox : PromptBoxController
{
    [SerializeField] GameObject tutorialPanel;
    [SerializeField] GameObject nextPromptBox;

    internal override void TriggerAction()
    {
        // Change Panel Transparency
        Color nextColor = Color.white;
        nextColor.a = 0f;
        tutorialPanel.GetComponent<Image>().color = nextColor;

        // Open Next Prompt Box
        nextPromptBox.SetActive(true);
        base.TriggerAction();
    }
}
