using UnityEngine;

public class CloseButton : ButtonEvent
{
    [SerializeField] private GameObject objectToClose;
    internal override void OnClick()
    {
        base.OnClick();
        objectToClose.SetActive(false);
    }
}
