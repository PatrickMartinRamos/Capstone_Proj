using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class WorldManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> worlds;
    [SerializeField] private int selectedWorldIndex = 0;
    [SerializeField] TextMeshProUGUI worldNameTxt;
    [SerializeField] private GameObject nextBtn, preBtn, selectBtn;
    private GameObject mainCam;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mainCam = GameObject.FindGameObjectWithTag("MainCamera");
        worldNameTxt.text = worlds[selectedWorldIndex].GetComponent<WorldSelection>().Selected();

        nextBtn.GetComponent<Button>().onClick.AddListener(SelectNextWorld);
        preBtn.GetComponent<Button>().onClick.AddListener(SelectPrevWorld);
        selectBtn.GetComponent<Button>().onClick.AddListener(EnterWorld);
    }

    // Update is called once per frame
    void Update()
    {
        if (worlds[selectedWorldIndex] == null) return;

        Vector3 direction = worlds[selectedWorldIndex].transform.position - mainCam.transform.position;

        if (direction == Vector3.zero) return; // Prevent NaN when target is at same position

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        mainCam.transform.rotation = Quaternion.Slerp(mainCam.transform.rotation, targetRotation, Time.deltaTime * 1f);
    }
    void SelectNextWorld()
    {
        Debug.Log("Next");
        selectedWorldIndex = Mathf.Clamp(selectedWorldIndex + 1, 0, worlds.Count-1);
        worldNameTxt.text = worlds[selectedWorldIndex].GetComponent<WorldSelection>().Selected();
    }
    void SelectPrevWorld()
    {
        Debug.Log("Prev");
        selectedWorldIndex = Mathf.Clamp(selectedWorldIndex - 1, 0, worlds.Count - 1);
        worldNameTxt.text = worlds[selectedWorldIndex].GetComponent<WorldSelection>().Selected();
    }
    void EnterWorld()
    {
        worlds[selectedWorldIndex].GetComponent<WorldSelection>().EnterWorld();
    }

}
