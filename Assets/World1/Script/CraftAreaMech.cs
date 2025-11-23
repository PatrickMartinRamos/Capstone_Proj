using UnityEngine;

public class CraftAreaMech : AreaContainer
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnEnable()
    {
        StageManager.Instance.craftArea = this.gameObject;
    }

}
