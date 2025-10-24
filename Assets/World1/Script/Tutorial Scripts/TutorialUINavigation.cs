using DG.Tweening;
using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Rendering;

public class TutorialUINavigation : MonoBehaviour
{
    [Header("PlayerStatus")]
    [SerializeField] private AlgebraicFoundationPlayerStatus playerStatus;

    [Header("CharacterAvatar")]
    [SerializeField] private GameObject charImage;
    [SerializeField] private GameObject designatedPos;
    [SerializeField] private float transitionDuration;
    private RectTransform charImageRectTrans;

    [Header("PromptBox")]
    [SerializeField] private GameObject promptBox;

    [Header("PromptTexts")]
    [SerializeField] private TextMeshProUGUI promptTextBox;
    [SerializeField] private List <string> squaringBinomialPrompts;
    [SerializeField] private List<string> radicalsPrompts;
    [SerializeField] private List<string> completingTheSquarePrompts;
    private int currentPromptIndex;

    private void Start()
    {
        charImageRectTrans = charImage.GetComponent<RectTransform>();
        MoveToPosition();
    }
    // Update is called once per frame
    void Update()
    {

    }

    void MoveToPosition()
    {
        InitiateTutorialPrompt();

        Sequence seq = DOTween.Sequence();
        seq.Append(charImageRectTrans.DOMove(designatedPos.GetComponent<RectTransform>().position, transitionDuration));
        seq.Append(promptBox.transform.DOScaleY(1, transitionDuration/2));

    }
    List<string> GetPromptList()
    {
        int curStage = playerStatus.currentStage;
        switch (curStage)
        {
            case 1:
                return squaringBinomialPrompts;
                case 2:
                return radicalsPrompts;
                case 3:
                return completingTheSquarePrompts;
        }
        return null;
    }
    void InitiateTutorialPrompt()
    {
        currentPromptIndex = 0;
        promptTextBox.text = GetPromptList()[currentPromptIndex];
    }

    // Functions for Tutorial Next Button
    public void NextButton()
    {
        StageManager.Instance.StartGame();
        //promptBox.GetComponent<RectTransform>().offsetMin = new Vector2(promptBox.GetComponent<RectTransform>().offsetMin.x, 1385.65f);
    }
    public void ChangePrompt()
    {
        currentPromptIndex++;
        promptTextBox.text = GetPromptList()[currentPromptIndex];
    }

}
