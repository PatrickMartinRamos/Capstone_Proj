using UnityEngine;

public class OpenButton : ButtonEvent
{
    [SerializeField] private GameObject objectToOpen;
    internal override void OnClick()
    {
        base.OnClick();
        objectToOpen.SetActive(true);
    }
}
