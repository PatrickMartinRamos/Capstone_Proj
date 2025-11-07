using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

/// <summary>
/// Manages the selection and display of worlds in the navigation UI.
/// </summary>
public class WorldManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> worlds;
    [SerializeField] private int selectedWorldIndex = 0;
    private string sceneName;
    [SerializeField] private TextMeshProUGUI worldNameTxt;
    [SerializeField] private GameObject nextBtn, preBtn, selectBtn, bg;
    private GameObject mainCam;

    [Header("Animation Settings")]
    [SerializeField] private float moveDistance = 2f;
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
        selectBtn.GetComponent<Button>().onClick.AddListener(ShowWorldStages);


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

        ScreenController.Instance.OnClickChangeFont();
        previousWorldIndex = selectedWorldIndex;
        selectedWorldIndex++;

        if (selectedWorldIndex >= worlds.Count)
            selectedWorldIndex = 0;
        HighlightCurrentWorld();
    }

    void SelectPrevWorld()
    {
        if (isAnimating) return;

        ScreenController.Instance.OnClickChangeFont();
        previousWorldIndex = selectedWorldIndex;
        selectedWorldIndex--;

        if (selectedWorldIndex < 0)
            selectedWorldIndex = worlds.Count - 1;

        HighlightCurrentWorld();
    }

    public void ShowWorldStages()
    {
        //if (isAnimating) return;
        var selectedWorld = worlds[selectedWorldIndex];
        //var worldName = selectedWorld.GetComponent<WorldSelection>().worldNameGet();
        var stageTransform = selectedWorld.GetComponent<WorldSelection>().OpenRectTransform();

        selectedWorld.GetComponent<WorldSelection>().OpenWorldStage(stageTransform);

        HideWorldSelectionUI();
        //Debug.Log($"[WorldManager] Show World: {worldName}, initializing its stages...");
    }

    public void CloseWorldStages()
    {
        var selectedWorld = worlds[selectedWorldIndex];
        var stageTransform = selectedWorld.GetComponent<WorldSelection>().OpenRectTransform();
        selectedWorld.GetComponent<WorldSelection>().CloseWorldStage(stageTransform);
        ShowWorldSelectionUI();
    }

    void HideWorldSelectionUI()
    {
        bg.GetComponent<RectTransform>().DOAnchorPosY(-639, 0.2f);
        nextBtn.GetComponent<RectTransform>().DOAnchorPosX(300, 0.2f);
        preBtn.GetComponent<RectTransform>().DOAnchorPosX(-636.11f, 0.2f);
        selectBtn.GetComponent<RectTransform>().DOAnchorPosY(-1400, 0.2f);
    }

    void ShowWorldSelectionUI()
    {
        bg.GetComponent<RectTransform>().DOAnchorPosY(27, 0.2f);
        nextBtn.GetComponent<RectTransform>().DOAnchorPosX(0, 0.2f);
        preBtn.GetComponent<RectTransform>().DOAnchorPosX(-0, 0.2f);
        selectBtn.GetComponent<RectTransform>().DOAnchorPosY(288.9998f, 0.2f);
    }


}
