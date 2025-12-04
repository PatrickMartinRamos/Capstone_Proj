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
    [ShowIf("isTutorial")]
    [SerializeField] private GameObject nextBtn, prevBtn; 
    private int currentPromptIndex = 0; bool isOnGuide = false;

    public void turnOffOnPointDown(bool trigger)
    {
        isOnGuide = trigger;
    }
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
        if(!isTutorial && !isOnGuide)
        NextPrompt();
    }

    public void NextPrompt()
    {
        if (tutorialPrompts.Count == 0 || currentPromptIndex > tutorialPrompts.Count-1)
            return;

        else if (currentPromptIndex == tutorialPrompts.Count - 1)
        {
            if (!isTutorial) EndTutorial();
            else return;
        }
            // Hide current prompt

        else 
        {
            tutorialPrompts[currentPromptIndex].SetActive(false);

            currentPromptIndex++;
            tutorialPrompts[currentPromptIndex].SetActive(true);
            if (isTutorial)
            {
                if (currentPromptIndex > 0 && !prevBtn.activeInHierarchy)
                    prevBtn.SetActive(true);
                if (currentPromptIndex == tutorialPrompts.Count - 1)
                    nextBtn.SetActive(false);
            }
        }

 
    }
    public void PrevPrompt()
    {
        if (tutorialPrompts.Count == 0 || currentPromptIndex < 0)
            return;

        else if (currentPromptIndex == 0)
        {
            if (!isTutorial) EndTutorial();
            else return;
        }
        // Hide current prompt

        else
        {
            tutorialPrompts[currentPromptIndex].SetActive(false);

            currentPromptIndex--;
            tutorialPrompts[currentPromptIndex].SetActive(true);
            if (isTutorial)
            {
                if (currentPromptIndex < tutorialPrompts.Count - 1 && !nextBtn.activeInHierarchy)
                    nextBtn.SetActive(true);
                if (currentPromptIndex == 0)
                    prevBtn.SetActive(false);
            }

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
