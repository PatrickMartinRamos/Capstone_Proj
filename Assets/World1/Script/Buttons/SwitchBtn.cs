using System.Collections.Generic;
using UnityEngine;

public class SwitchBtn : ButtonEvent
{
    [SerializeField] private List<GameObject> objectToClose = new();
    [SerializeField] private List<GameObject> objectToOpen = new();

    internal override void OnClick()
    {
        base.OnClick();
        foreach (GameObject obj in objectToOpen)
        {
            obj.SetActive(true);
        }

        foreach (GameObject obj in objectToClose)
        {
            obj.SetActive(false);
        }

    }
}
