using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class WorldManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> worlds;
    [SerializeField] private int selectedWorldIndex = 0;
    [SerializeField] private TextMeshProUGUI worldNameTxt;
    [SerializeField] private GameObject nextBtn, preBtn, selectBtn;
    private GameObject mainCam;

    [Header("Animation Settings")]
    [SerializeField] private float moveDistance = 2f;  // how high the selected world moves
    [SerializeField] private float moveDuration = 0.5f;

    private int previousWorldIndex = -1;
    private Dictionary<GameObject, float> baseYPositions = new Dictionary<GameObject, float>();
    private bool isAnimating = false;

    void Start()
    {
        mainCam = GameObject.FindGameObjectWithTag("MainCamera");

        // Record base Y positions
        foreach (var world in worlds)
        {
            baseYPositions[world] = world.transform.position.y;
        }

        worldNameTxt.text = worlds[selectedWorldIndex].GetComponent<WorldSelection>().Selected();

        nextBtn.GetComponent<Button>().onClick.AddListener(SelectNextWorld);
        preBtn.GetComponent<Button>().onClick.AddListener(SelectPrevWorld);
        selectBtn.GetComponent<Button>().onClick.AddListener(EnterWorld);

        HighlightCurrentWorld();
    }

    void HighlightCurrentWorld()
    {
        isAnimating = true; // lock input during animation

        Sequence seq = DOTween.Sequence();

        // Move previous world back to base
        if (previousWorldIndex >= 0 && previousWorldIndex < worlds.Count)
        {
            GameObject prevWorld = worlds[previousWorldIndex];
            prevWorld.transform.DOKill();
            seq.Join(prevWorld.transform.DOMoveY(baseYPositions[prevWorld], moveDuration)
                .SetEase(Ease.InOutSine));
        }

        // Move current world upward
        GameObject currentWorld = worlds[selectedWorldIndex];
        currentWorld.transform.DOKill();
        seq.Join(currentWorld.transform.DOMoveY(baseYPositions[currentWorld] + moveDistance, moveDuration)
            .SetEase(Ease.OutSine));

        worldNameTxt.text = currentWorld.GetComponent<WorldSelection>().Selected();

        // Unlock input when finished
        seq.OnComplete(() => isAnimating = false);
    }

    void SelectNextWorld()
    {
        if (isAnimating) return;

        previousWorldIndex = selectedWorldIndex;
        selectedWorldIndex++;

        if (selectedWorldIndex >= worlds.Count)
            selectedWorldIndex = 0;

        HighlightCurrentWorld();
    }

    void SelectPrevWorld()
    {
        if (isAnimating) return;

        previousWorldIndex = selectedWorldIndex;
        selectedWorldIndex--;

        if (selectedWorldIndex < 0)
            selectedWorldIndex = worlds.Count - 1;

        HighlightCurrentWorld();
    }

    void EnterWorld()
    {
        if (isAnimating) return; // optional safety check
        worlds[selectedWorldIndex].GetComponent<WorldSelection>().EnterWorld();
    }
}
