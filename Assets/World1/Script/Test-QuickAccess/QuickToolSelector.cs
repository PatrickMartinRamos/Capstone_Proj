using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class QuickToolSelector : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject radialMenuPrefab;
    [SerializeField] private Transform uiCanvas;
    [SerializeField] private float selectionRadius = 100f;

    [Header("Icons (Order: Add, Subtract, Multiply, Divide)")]
    [SerializeField] private List<Image> toolIcons;

    private GameObject activeMenu;
    private int hoveredIndex = -1;
    private bool isSelecting;
    private Vector2 startTouchPos;

    private void Update()
    {
        var input = InputManager.Instance;

        // Begin hold
        if (input.IsTouching() && !isSelecting)
        {
            startTouchPos = input.StartTouchPosition();
            OpenMenu(startTouchPos);
            isSelecting = true;
        }

        // Drag detection
        if (isSelecting && input.IsTouching())
        {
            Vector2 currentPos = input.GetTouchPosition();
            UpdateHover(currentPos);
        }

        // Release
        if (isSelecting && input.TouchEnded())
        {
            SelectTool();
            CloseMenu();
            isSelecting = false;
        }
    }

    private void OpenMenu(Vector2 position)
    {
        activeMenu = Instantiate(radialMenuPrefab, uiCanvas);
        activeMenu.transform.position = position;

        toolIcons = new List<Image>(activeMenu.GetComponentsInChildren<Image>());
        hoveredIndex = -1;
    }

    private void CloseMenu()
    {
        if (activeMenu != null)
            Destroy(activeMenu);
    }

    private void UpdateHover(Vector2 currentPos)
    {
        Vector2 dir = currentPos - startTouchPos;
        if (dir.magnitude < selectionRadius / 2f)
        {
            Highlight(-1);
            hoveredIndex = -1;
            return;
        }

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if (angle < 0) angle += 360f;

        // Divide 360° into 4 slices
        int index = Mathf.FloorToInt(angle / 90f);

        if (index != hoveredIndex)
        {
            Highlight(index);
            hoveredIndex = index;
        }
    }

    private void Highlight(int index)
    {
        for (int i = 0; i < toolIcons.Count; i++)
        {
            toolIcons[i].color = (i == index) ? Color.yellow : Color.white;
        }
    }

    private void SelectTool()
    {
        if (hoveredIndex == -1) return;

        switch (hoveredIndex)
        {
            case 0: Debug.Log("Tool Selected: Addition"); break;
            case 1: Debug.Log("Tool Selected: Subtraction"); break;
            case 2: Debug.Log("Tool Selected: Multiplication"); break;
            case 3: Debug.Log("Tool Selected: Division"); break;
        }

        // Here you can trigger your astronaut’s tool-equip animation, e.g.:
        // ToolManager.Instance.EquipTool((ToolType)hoveredIndex);
    }
}
