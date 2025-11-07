using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using Unity.VisualScripting; // for Pointer & Touch input

public class TutorialPromptBox : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private GameObject tutorialPanel;
    [SerializeField] private List<GameObject> tutorialPrompts = new List<GameObject>();

    private int currentPromptIndex = 0;

    private void Awake()
    {
        if (tutorialPrompts.Count == 0 && tutorialPanel != null)
        {
            foreach (Transform child in tutorialPanel.transform)
                tutorialPrompts.Add(child.gameObject);
        }
    }

    private void Start()
    {
        for (int i = 0; i < tutorialPrompts.Count; i++)
            tutorialPrompts[i].SetActive(i == 0);
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        NextPrompt();
    }

    private void NextPrompt()
    {
        if (tutorialPrompts.Count == 0)
            return;

        // Hide current prompt
        tutorialPrompts[currentPromptIndex].SetActive(false);

        currentPromptIndex++;

        if (currentPromptIndex < tutorialPrompts.Count)
        {
            // Show next
            tutorialPrompts[currentPromptIndex].SetActive(true);
        }
        else
        {
            EndTutorial();
        }
    }

    private void EndTutorial()
    {
        //Dotween scale
        tutorialPanel.SetActive(false);
        gameObject.SetActive(false);
    }
}
