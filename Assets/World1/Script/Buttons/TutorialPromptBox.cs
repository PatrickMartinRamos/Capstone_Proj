using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using Unity.VisualScripting; // for Pointer & Touch input

public class TutorialPromptBox : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private GameObject tutorialPanel, nextTutorial;
    [SerializeField] private GameObject astronaut;
    [SerializeField] private List<GameObject> tutorialPrompts = new List<GameObject>();
    [SerializeField] private bool isTutorial = false;

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
        if(!isTutorial)
        NextPrompt();
    }

    public void NextPrompt()
    {
        if (tutorialPrompts.Count == 0 || currentPromptIndex == tutorialPrompts.Count-1)
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
            if (!isTutorial)
                EndTutorial();
        }
    }
    public void PrevPrompt()
    {
        if (tutorialPrompts.Count == 0 || currentPromptIndex == 0)
            return;

        // Hide current prompt
        tutorialPrompts[currentPromptIndex].SetActive(false);

        currentPromptIndex--;

        if (currentPromptIndex >= 0)
        {
            // Show prev
            tutorialPrompts[currentPromptIndex].SetActive(true);
        }

        else
        {
            if (!isTutorial)
                EndTutorial();
        }
    }

    public void EndTutorial()
    {
        //Dotween scale
        if(nextTutorial != null) nextTutorial.SetActive(true);
        astronaut.SetActive(false);

        tutorialPanel.SetActive(false);
        gameObject.SetActive(false);
    }
}
