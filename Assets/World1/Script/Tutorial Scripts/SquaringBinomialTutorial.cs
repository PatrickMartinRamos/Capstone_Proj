using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SquaringBinomialTutorial : MonoBehaviour
{
    [Header("CharacterAvatar")]
    [SerializeField] private GameObject charImage;
    [SerializeField] private GameObject designatedPos;
    [SerializeField] private float transitionDuration;
    private RectTransform charImageRectTrans;

    [Header("PromptBox")]
    [SerializeField] private GameObject promptBox;

    [Header("PromptTexts")]
    [SerializeField] private TextMeshProUGUI promptTextBox;
    [SerializeField] private List<string> squaringBinomialPrompts;
    private int currentPromptIndex;

    private void Start()
    {
        charImageRectTrans = charImage.GetComponent<RectTransform>();
        MoveToPosition();
    }
    void MoveToPosition()
    {
        InitiateTutorialPrompt();

        Sequence seq = DOTween.Sequence();
        seq.Append(charImageRectTrans.DOMove(designatedPos.GetComponent<RectTransform>().position, transitionDuration));
        seq.Append(promptBox.transform.DOScaleY(1, transitionDuration / 2));
    }
    void InitiateTutorialPrompt()
    {
        currentPromptIndex = 0;
        promptTextBox.text = squaringBinomialPrompts[currentPromptIndex];
    }
    void ShowObjectivePrompt()
    {
        StageManager.Instance.StartGame();
    }
    void ShowGivenPrompt()
    {
        promptBox.GetComponent<RectTransform>().offsetMin = new Vector2(promptBox.GetComponent<RectTransform>().offsetMin.x, 1385.65f);
    }
    public void ChangePrompt()
    {
        currentPromptIndex++;
        promptTextBox.text = squaringBinomialPrompts[currentPromptIndex];
        switch (currentPromptIndex)
        {
            case 1: 
                ShowObjectivePrompt(); break;
            case 2:
                ShowGivenPrompt(); break;
            default:
                break;
        }
    }
}
